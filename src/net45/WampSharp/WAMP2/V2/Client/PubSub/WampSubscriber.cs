using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal class WampSubscriber<TMessage> : IWampSubscriber<TMessage>, IWampTopicSubscriptionProxy,
        IWampSubscriberError<TMessage>
    {
        private readonly WampIdMapper<SubscribeRequest> mPendingSubscriptions =
            new WampIdMapper<SubscribeRequest>();

        private readonly WampIdMapper<UnsubscribeRequest> mPendingUnsubscriptions =
            new WampIdMapper<UnsubscribeRequest>();

        private readonly IWampServerProxy mProxy;
        private readonly IWampFormatter<TMessage> mFormatter;

        private readonly ConcurrentDictionary<long, Subscription> mSubscriptionIdToSubscription =
            new ConcurrentDictionary<long, Subscription>();

        public WampSubscriber(IWampServerProxy proxy,
                              IWampFormatter<TMessage> formatter)
        {
            mProxy = proxy;
            mFormatter = formatter;
        }

        public Task<IDisposable> Subscribe(IWampRawTopicSubscriber subscriber, object options, string topicUri)
        {
            SubscribeRequest request = new SubscribeRequest(subscriber, options, topicUri);
            long requestId = mPendingSubscriptions.Add(request);
            request.RequestId = requestId;

            mProxy.Subscribe(requestId, options, topicUri);

            return request.Task;
        }

        private Task Unsubscribe(long subscriptionId)
        {
            UnsubscribeRequest request = new UnsubscribeRequest(subscriptionId);
            long requestId = mPendingUnsubscriptions.Add(request);
            request.RequestId = requestId;
            mProxy.Unsubscribe(requestId, subscriptionId);

            return request.Task;
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            SubscribeRequest request;
            
            if (mPendingSubscriptions.TryRemove(requestId, out request))
            {
                mSubscriptionIdToSubscription[subscriptionId] =
                    new Subscription(subscriptionId,
                                     request.Subscriber,
                                     request.Options,
                                     request.TopicUri);

                request.Complete(new UnsubscribeDisposable(this, subscriptionId));
            }
        }

        public void Unsubscribed(long requestId, long subscriptionId)
        {
            UnsubscribeRequest request;

            if (mPendingUnsubscriptions.TryRemove(requestId, out request))
            {
                request.Complete();
                Subscription subscription;
                mSubscriptionIdToSubscription.TryRemove(subscriptionId, out subscription);
            }
        }

        public void SubscribeError(long requestId, TMessage details, string error)
        {
            throw new NotImplementedException();
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            throw new NotImplementedException();
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeError(long requestId, TMessage details, string error)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            throw new NotImplementedException();
        }

        public void Event(long subscriptionId, long publicationId, TMessage details)
        {
            InnerEvent(subscriptionId, 
                subscriber => subscriber.Event(Formatter, publicationId, details));
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments)
        {
            InnerEvent(subscriptionId,
                       subscriber => subscriber.Event(Formatter,
                                                      publicationId,
                                                      details,
                                                      arguments));
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            InnerEvent(subscriptionId,
                       subscriber => subscriber.Event(Formatter,
                                                      publicationId,
                                                      details,
                                                      arguments,
                                                      argumentsKeywords));
        }

        private void InnerEvent(long subscriptionId, Action<IWampRawTopicSubscriber> action)
        {
            Subscription subscription;

            if (mSubscriptionIdToSubscription.TryGetValue(subscriptionId, out subscription))
            {
                action(subscription.Subscriber);
            }
        }

        private IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mFormatter;
            }
        }

        private class BaseSubscription
        {
            private readonly IWampRawTopicSubscriber mSubscriber;
            private readonly object mOptions;
            private readonly string mTopicUri;

            public BaseSubscription(IWampRawTopicSubscriber subscriber, object options, string topicUri)
            {
                mSubscriber = subscriber;
                mOptions = options;
                mTopicUri = topicUri;
            }

            public IWampRawTopicSubscriber Subscriber
            {
                get
                {
                    return mSubscriber;
                }
            }

            public object Options
            {
                get
                {
                    return mOptions;
                }
            }

            public string TopicUri
            {
                get
                {
                    return mTopicUri;
                }
            }
        }

        private class SubscribeRequest : BaseSubscription
        {
            private readonly TaskCompletionSource<IDisposable> mTask =
                new TaskCompletionSource<IDisposable>();

            public SubscribeRequest(IWampRawTopicSubscriber subscriber, object options, string topicUri) : 
                base(subscriber, options, topicUri)
            {
            }

            public long RequestId
            {
                get; 
                set;
            }

            public Task<IDisposable> Task
            {
                get
                {
                    return mTask.Task;
                }
            }

            public void Complete(IDisposable disposable)
            {
                mTask.TrySetResult(disposable);
            }
        }

        private class Subscription : BaseSubscription
        {
            private readonly long mSubscriptionId;

            public Subscription(long subscriptionId, IWampRawTopicSubscriber subscriber, object options, string topicUri) : 
                base(subscriber, options, topicUri)
            {
                mSubscriptionId = subscriptionId;
            }

            public long SubscriptionId
            {
                get
                {
                    return mSubscriptionId;
                }
            }
        }

        private class UnsubscribeRequest
        {
            private readonly long mSubscriptionId;
            private readonly TaskCompletionSource<bool> mTaskCompletionSource =
                new TaskCompletionSource<bool>();

            public UnsubscribeRequest(long subscriptionId)
            {
                mSubscriptionId = subscriptionId;
            }

            public long SubscriptionId
            {
                get
                {
                    return mSubscriptionId;
                }
            }

            public Task Task
            {
                get
                {
                    return mTaskCompletionSource.Task;
                }
            }

            public long RequestId { get; set; }

            public void Complete()
            {
                mTaskCompletionSource.SetResult(true);
            }
        }

        private class UnsubscribeDisposable : IDisposable
        {
            private readonly WampSubscriber<TMessage> mParent;
            private readonly long mSubscriptionId;

            public UnsubscribeDisposable(WampSubscriber<TMessage> parent, long subscriptionId)
            {
                mParent = parent;
                mSubscriptionId = subscriptionId;
            }

            public void Dispose()
            {
                mParent.Unsubscribe(mSubscriptionId);
            }
        }
    }
}