using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;

namespace WampSharp.PubSub.Server
{
    public class WampTopic : IWampTopic
    {
        #region Fields

        private readonly Subject<WampNotification> mSubject =
            new Subject<WampNotification>();

        private readonly ConcurrentDictionary<string, Subscription> mSessionIdToSubscription =
            new ConcurrentDictionary<string, Subscription>();

        private readonly string mTopicUri;
        private readonly IDisposable mContainerDisposable;
        private readonly bool mPersistent;
        private readonly object mLock = new object();
        private bool mDisposed = false;

        #endregion

        #region Constructor

        public WampTopic(string topicUri, IDisposable containerDisposable, bool persistent = false)
        {
            mTopicUri = topicUri;
            mContainerDisposable = containerDisposable;
            mPersistent = persistent;
        }

        #endregion

        #region IWampTopic methods

        public void OnNext(WampNotification value)
        {
            mSubject.OnNext(value);
        }

        public void OnNext(object value)
        {
            mSubject.OnNext(new WampNotification(value));
        }

        public void OnError(Exception error)
        {
            mSubject.OnError(error);
        }

        public void OnCompleted()
        {
            mSubject.OnCompleted();
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public bool Persistent
        {
            get
            {
                return mPersistent;
            }
        }

        public event EventHandler<WampSubscriptionAddedEventArgs> SubscriptionAdded;
        public event EventHandler<WampSubscriptionRemovedEventArgs> SubscriptionRemoved;

        public void Unsubscribe(string sessionId)
        {
            Subscription subscription;

            if (mSessionIdToSubscription.TryRemove(sessionId, out subscription))
            {
                subscription.Dispose();
            }
        }

        public IDisposable Subscribe(IObserver<object> observer)
        {
            WampObserver casted = observer as WampObserver;

            if (casted == null)
            {
                return GetSubscriptionDisposable(mSubject.Select(x => x.Event),
                                                 observer);
            }
            else
            {
                string sessionId = casted.SessionId;

                IObservable<object> relevantMessages =
                    mSubject.Where(x => ShouldPublishMessage(x, sessionId))
                            .Select(x => x.Event);

                CompositeDisposable result =
                    GetSubscriptionDisposable(relevantMessages, observer);
                
                result.Add(Disposable.Create(() => OnClientUnsubscribed(sessionId)));
                
                mSessionIdToSubscription[sessionId] = new Subscription(this, casted, result);

                RaiseSubscriptionAdded(casted);

                return result;
            }
        }

        public void Dispose()
        {
            lock (mLock)
            {
                if (!mDisposed)
                {
                    mDisposed = true;
                    mContainerDisposable.Dispose();

                    if (mSubject != null)
                    {
                        mSubject.OnCompleted();
                        mSubject.Dispose();
                    }

                    foreach (Subscription subscription in mSessionIdToSubscription.Values)
                    {
                        subscription.Dispose();
                    }

                    mSessionIdToSubscription.Clear();
                }
            }
        }

        #endregion

        #region Private Methods

        private void RaiseSubscriptionAdded(WampObserver observer)
        {
            EventHandler<WampSubscriptionAddedEventArgs> added = 
                SubscriptionAdded;

            if (added != null)
            {
                added(this, new WampSubscriptionAddedEventArgs(observer.SessionId,
                                                               observer));
            }
        }

        private void OnClientUnsubscribed(string sessionId)
        {
            RaiseSubscriptionRemoved(sessionId);
        }

        private void RaiseSubscriptionRemoved(string sessionId)
        {
            EventHandler<WampSubscriptionRemovedEventArgs> subscriptionRemoved = 
                SubscriptionRemoved;

            if (subscriptionRemoved != null)
            {
                subscriptionRemoved(this,
                                    new WampSubscriptionRemovedEventArgs(sessionId));
            }
        }

        private CompositeDisposable GetSubscriptionDisposable(IObservable<object> observable, IObserver<object> observer)
        {
            return new CompositeDisposable(observable.Subscribe(observer),
                                           Disposable.Create(() => OnDispose()));
        }

        private void OnDispose()
        {
            // TODO: A race condition can occur here if at the same
            // TODO: time a non-persistent topic self-destructs itself
            // TODO: And a client tries to subscribe to the same non-persistant
            // TODO: topic
            if (!mSubject.HasObservers && !Persistent)
            {
                // Self-destruct.
                Dispose();
            }
        }

        private static bool ShouldPublishMessage(WampNotification wampNotification, string sessionId)
        {
            return (!wampNotification.Eligible.Any() &&
                    !wampNotification.Excluded.Contains(sessionId)) ||
                   wampNotification.Eligible.Contains(sessionId);
        }

        #endregion

        #region Nested Classes

        private class Subscription : IDisposable
        {
            private readonly IWampTopic mTopic;
            private readonly WampObserver mObserver;
            private readonly IDisposable mDisposable;

            public Subscription(IWampTopic topic, WampObserver observer, IDisposable disposable)
            {
                mTopic = topic;
                mObserver = observer;
                mDisposable = disposable;
                TrackConnection();
            }

            private void TrackConnection()
            {
                IWampConnectionMonitor monitor = 
                    mObserver.Client as IWampConnectionMonitor;

                monitor.ConnectionClosed += OnConnectionClosed;
            }

            private void OnConnectionClosed(object sender, EventArgs e)
            {
                // TODO: Spaghetti code - We call this in order to remove
                // the subscription from the dictionary, and it calls
                // Dispose...
                mTopic.Unsubscribe(Observer.SessionId);
            }

            private WampObserver Observer
            {
                get
                {
                    return mObserver;
                }
            }

            public void Dispose()
            {
                mDisposable.Dispose();

                IWampConnectionMonitor monitor =
                    mObserver.Client as IWampConnectionMonitor;

                monitor.ConnectionClosed -= OnConnectionClosed;
            }
        }

        #endregion
    }
}