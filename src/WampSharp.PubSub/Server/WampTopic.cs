using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace WampSharp.PubSub.Server
{
    public class WampTopic<TMessage> : 
        ISubject<WampNotification<TMessage>, TMessage>,
        ISubject<TMessage>
    {
        private readonly Subject<WampNotification<TMessage>> mSubject =
            new Subject<WampNotification<TMessage>>();

        private readonly ConcurrentDictionary<string, Subscription> mSessionIdToSubscription =
            new ConcurrentDictionary<string, Subscription>();

        public void OnNext(WampNotification<TMessage> value)
        {
            mSubject.OnNext(value);
        }

        public void OnNext(TMessage value)
        {
            mSubject.OnNext(new WampNotification<TMessage>(value));
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

        public IDisposable Subscribe(IObserver<TMessage> observer)
        {
            WampObserver<TMessage> casted = observer as WampObserver<TMessage>;

            string sessionId = casted.SessionId;

            IObservable<TMessage> relevantMessages =
                mSubject.Where(x => ShouldPublishMessage(x, sessionId))
                        .Select(x => x.Event);

            CompositeDisposable result =
                new CompositeDisposable(relevantMessages.Subscribe(observer),
                                        Disposable.Create(() => OnDispose()));

            mSessionIdToSubscription[sessionId] = new Subscription(casted, result);

            return result;
        }

        private void OnDispose()
        {
        }

        private static bool ShouldPublishMessage(WampNotification<TMessage> wampNotification, string sessionId)
        {
            return (!wampNotification.Eligible.Any() &&
                    !wampNotification.Excluded.Contains(sessionId)) ||
                   wampNotification.Eligible.Contains(sessionId);
        }

        private class Subscription : IDisposable
        {
            private readonly WampObserver<TMessage> mObserver;
            private readonly IDisposable mDisposable;

            public Subscription(WampObserver<TMessage> observer, IDisposable disposable)
            {
                mObserver = observer;
                mDisposable = disposable;
            }

            public WampObserver<TMessage> Observer
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