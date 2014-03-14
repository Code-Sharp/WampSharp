using System;
using System.Collections.Concurrent;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    public class RawWampTopic<TMessage> : IRawWampTopic<TMessage>, IWampTopicSubscriber
    {
        private readonly ConcurrentDictionary<long, Subscription> mSesssionIdToSubscription =
            new ConcurrentDictionary<long, Subscription>();

        private readonly IWampBinding<TMessage> mBinding; 
        private readonly IWampEventSerializer<TMessage> mSerializer;
        private readonly ISubject<WampMessage<TMessage>> mSubject = new Subject<WampMessage<TMessage>>();
        private readonly long mSubscriptionId;
        private readonly string mTopicUri;

        public RawWampTopic(string topicUri, long subscriptionId, IWampEventSerializer<TMessage> serializer, IWampBinding<TMessage> binding)
        {
            mSerializer = serializer;
            mTopicUri = topicUri;
            mBinding = binding;
            mSubscriptionId = subscriptionId;
        }

        public void Event(long publicationId, object details)
        {
            WampMessage<TMessage> message =
                mSerializer.Event(mSubscriptionId, publicationId, details);

            Publish(message);
        }

        public void Event(long publicationId, object details, object[] arguments)
        {
            WampMessage<TMessage> message =
                mSerializer.Event(mSubscriptionId, publicationId, details, arguments);

            Publish(message);
        }

        public void Event(long publicationId, object details, object[] arguments, object argumentsKeywords)
        {
            WampMessage<TMessage> message =
                mSerializer.Event(mSubscriptionId, publicationId, details, arguments, argumentsKeywords);

            Publish(message);
        }

        private void Publish(WampMessage<TMessage> message)
        {
            WampMessage<TMessage> raw = mBinding.GetRawMessage(message);
            mSubject.OnNext(raw);
        }

        public long SubscriptionId
        {
            get
            {
                return mSubscriptionId;
            }
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }

        public void Subscribe(IWampSubscriber subscriber, TMessage options)
        {
            IWampClient<TMessage> client = subscriber as IWampClient<TMessage>;

            RemoteObserver observer = new RemoteObserver(client);
            
            IDisposable disposable = mSubject.Subscribe(observer);
            
            Subscription subscription = new Subscription(this, client, disposable);

            mSesssionIdToSubscription.TryAdd(client.Session, subscription);
        }

        public void Unsubscribe(IWampSubscriber subscriber)
        {
            IWampClient<TMessage> client = subscriber as IWampClient<TMessage>;

            Subscription subscription;
            
            if (mSesssionIdToSubscription.TryRemove(client.Session, out subscription))
            {
                subscription.Dispose();
            }
        }

        private class Subscription : IDisposable
        {
            private readonly RawWampTopic<TMessage> mParent;
            private readonly IWampClient<TMessage> mClient;
            private readonly IDisposable mDisposable;

            public Subscription(RawWampTopic<TMessage> parent, IWampClient<TMessage> client, IDisposable disposable)
            {
                mParent = parent;
                mClient = client;
                mDisposable = disposable;

                IWampConnectionMonitor monitor = mClient as IWampConnectionMonitor;
                monitor.ConnectionClosed += OnConnectionClosed;
            }

            private void OnConnectionClosed(object sender, EventArgs e)
            {
                mParent.Unsubscribe(mClient as IWampSubscriber);
                IWampConnectionMonitor monitor = sender as IWampConnectionMonitor;
                monitor.ConnectionClosed -= OnConnectionClosed;
            }

            public void Dispose()
            {
                mDisposable.Dispose();
            }
        }

        private class RemoteObserver : IObserver<WampMessage<TMessage>>
        {
            private readonly IWampRawClient<TMessage> mClient;

            public RemoteObserver(IWampRawClient<TMessage> client)
            {
                mClient = client;
            }

            public void OnNext(WampMessage<TMessage> value)
            {
                mClient.Message(value);
            }

            public void OnError(Exception error)
            {
            }

            public void OnCompleted()
            {
            }
        }
    }
}