using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Serialization;

namespace WampSharp.PubSub.Client
{
    public class WampPubSubSubject<TMessage, TEvent> : ISubject<TEvent>
    {
        private readonly Subject<TEvent> mSubject = new Subject<TEvent>(); 
        private readonly string mTopicUri;
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly object mLock = new object();
        private readonly IWampServer mServerProxy;
        private bool mSubscribed;

        public WampPubSubSubject(string topicUri, IWampFormatter<TMessage> formatter, IWampServerProxyFactory<TMessage> serverProxyFactory)
        {
            mTopicUri = topicUri;
            mFormatter = formatter;
            mServerProxy = serverProxyFactory.Create(new WampPubSubClient(this));
        }

        public void OnNext(TEvent value)
        {
            mServerProxy.Publish(null, mTopicUri, value, false);
        }

        public void OnError(Exception error)
        {
            // Won't happen :)
        }

        public void OnCompleted()
        {
            mSubject.OnCompleted();
            OnUnsubscribe();
        }

        public IDisposable Subscribe(IObserver<TEvent> observer)
        {
            lock (mLock)
            {
                bool hasObservers = mSubject.HasObservers;
                
                IDisposable disposable = mSubject.Subscribe(observer);

                if (!hasObservers && mSubject.HasObservers)
                {
                    mServerProxy.Subscribe(null, mTopicUri);
                    mSubscribed = true;
                }

                return new CompositeDisposable(disposable,
                                               Disposable.Create(() => OnUnsubscribe()));
            }
        }

        private void OnUnsubscribe()
        {
            lock (mLock)
            {
                if (!mSubject.HasObservers)
                {
                    if (mSubscribed)
                    {
                        mServerProxy.Unsubscribe(null, mTopicUri);
                        mSubscribed = false;
                    }
                }                
            }
        }

        private void EventArrived(TMessage @event)
        {
            TEvent deserialized = mFormatter.Deserialize<TEvent>(@event);
            mSubject.OnNext(deserialized);
        }

        private class WampPubSubClient : IWampPubSubClient<TMessage>
        {
            private readonly WampPubSubSubject<TMessage, TEvent> mParent;

            public WampPubSubClient(WampPubSubSubject<TMessage, TEvent> parent)
            {
                mParent = parent;
            }

            public void Event(string topicUri, TMessage @event)
            {
                if (topicUri == mParent.mTopicUri)
                {
                    mParent.EventArrived(@event);
                }
            }
        }
    }
}