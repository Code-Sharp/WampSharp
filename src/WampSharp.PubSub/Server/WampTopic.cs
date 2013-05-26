using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;

namespace WampSharp.PubSub.Server
{
    public class WampTopic : 
        ISubject<WampNotification, object>,
        ISubject<object>
    {
        private readonly Subject<WampNotification> mSubject =
            new Subject<WampNotification>();

        private readonly ConcurrentDictionary<string, Subscription> mSessionIdToSubscription =
            new ConcurrentDictionary<string, Subscription>();

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

                mSessionIdToSubscription[sessionId] = new Subscription(this, casted, result);

                return result;
            }
        }

        private CompositeDisposable GetSubscriptionDisposable(IObservable<object> observable, IObserver<object> observer)
        {
            return new CompositeDisposable(observable.Subscribe(observer),
                                           Disposable.Create(() => OnDispose()));
        }

        private void OnDispose()
        {
        }

        private static bool ShouldPublishMessage(WampNotification wampNotification, string sessionId)
        {
            return (!wampNotification.Eligible.Any() &&
                    !wampNotification.Excluded.Contains(sessionId)) ||
                   wampNotification.Eligible.Contains(sessionId);
        }

        private class Subscription : IDisposable
        {
            private readonly WampTopic mTopic;
            private readonly WampObserver mObserver;
            private readonly IDisposable mDisposable;

            public Subscription(WampTopic topic, WampObserver observer, IDisposable disposable)
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

                IWampConnectionMonitor monitor = sender as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnConnectionClosed;
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
            }
        }
    }
}