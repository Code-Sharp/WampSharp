using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.V1.PubSub.Server.EventArgs;
using WampSharp.V1.PubSub.Server.Interfaces;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// An implementation of <see cref="IWampTopic"/>.
    /// </summary>
    public class WampTopic : IWampTopic
    {
        #region Fields

        private readonly Subject<WampNotification> mSubject =
            new Subject<WampNotification>();

        private readonly ConcurrentDictionary<string, Subscription> mSessionIdToSubscription =
            new ConcurrentDictionary<string, Subscription>();

        private readonly string mTopicUri;
        private readonly bool mPersistent;
        private readonly object mLock = new object();
        private bool mDisposing = false;
        private bool mDisposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampTopic"/>.
        /// </summary>
        /// <param name="topicUri">The uri of the topic.</param>
        /// <param name="persistent">A value indicating whether the topic is persistent.</param>
        public WampTopic(string topicUri, bool persistent = false)
        {
            mTopicUri = topicUri;
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

        public bool HasObservers
        {
            get
            {
                return mSubject.HasObservers;
            }
        }

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;
        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving;

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;
        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved;
        public event EventHandler TopicEmpty;

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

            IDisposable result;

            if (casted == null)
            {
                IObservable<object> events = mSubject.Select(x => x.Event);

                result = 
                    events.Subscribe(observer);

                result = GetSubscriptionDisposable(result);
            }
            else
            {
                RaiseSubscriptionAdding(casted);

                string sessionId = casted.SessionId;

                IObservable<object> relevantMessages =
                    mSubject.Where(x => ShouldPublishMessage(x, sessionId))
                            .Select(x => x.Event);

                IDisposable observerDisposable =
                    relevantMessages.Subscribe(observer);

                result =
                    new CompositeDisposable
                        (Disposable.Create(() => OnWampObserverLeaving(sessionId)),
                        observerDisposable,
                        Disposable.Create(() => OnWampObserverLeft(sessionId)));
                
                result =
                    GetSubscriptionDisposable(result);

                mSessionIdToSubscription[sessionId] = 
                    new Subscription(this, casted, result);

                RaiseSubscriptionAdded(casted);
            }

            return result;
        }

        public void Dispose()
        {
            lock (mLock)
            {
                if (mDisposed)
                {
                    throw new ObjectDisposedException("topic");
                }

                if (!mDisposing)
                {
                    mDisposing = true;

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

                    mDisposing = false;
                    mDisposed = true;
                }
            }
        }

        #endregion

        #region Private Methods

        private IDisposable GetSubscriptionDisposable(IDisposable disposable)
        {
            return new CompositeDisposable(disposable,
                                           Disposable.Create(() => OnObserverLeft()));
        }

        private void OnObserverLeft()
        {
            if (!HasObservers)
            {
                RaiseTopicEmpty();
            }
        }

        private void OnWampObserverLeft(string sessionId)
        {
            RaiseSubscriptionRemoved(sessionId);
        }

        private void OnWampObserverLeaving(string sessionId)
        {
            RaiseSubscriptionRemoving(sessionId);
        }

        private static bool ShouldPublishMessage(WampNotification wampNotification, string sessionId)
        {
            return (!wampNotification.Eligible.Any() &&
                    !wampNotification.Excluded.Contains(sessionId)) ||
                   wampNotification.Eligible.Contains(sessionId);
        }

        #endregion

        #region Event Raising

        private void RaiseTopicEmpty()
        {
            EventHandler topicEmpty = TopicEmpty;

            if (topicEmpty != null)
            {
                topicEmpty(this, EventArgs.Empty);
            }
        }

        private void RaiseSubscriptionAdding(WampObserver observer)
        {
            EventHandler<WampSubscriptionAddEventArgs> adding =
                SubscriptionAdding;

            if (adding != null)
            {
                adding(this, new WampSubscriptionAddEventArgs(observer.SessionId,
                                                              observer));
            }
        }

        private void RaiseSubscriptionAdded(WampObserver observer)
        {
            EventHandler<WampSubscriptionAddEventArgs> added =
                SubscriptionAdded;

            if (added != null)
            {
                added(this, new WampSubscriptionAddEventArgs(observer.SessionId,
                                                               observer));
            }
        }

        private void RaiseSubscriptionRemoving(string sessionId)
        {
            EventHandler<WampSubscriptionRemoveEventArgs> subscriptionRemoving =
                SubscriptionRemoving;

            if (subscriptionRemoving != null)
            {
                subscriptionRemoving(this,
                                    new WampSubscriptionRemoveEventArgs(sessionId));
            }
        }

        private void RaiseSubscriptionRemoved(string sessionId)
        {
            EventHandler<WampSubscriptionRemoveEventArgs> subscriptionRemoved =
                SubscriptionRemoved;

            if (subscriptionRemoved != null)
            {
                subscriptionRemoved(this,
                                    new WampSubscriptionRemoveEventArgs(sessionId));
            }
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