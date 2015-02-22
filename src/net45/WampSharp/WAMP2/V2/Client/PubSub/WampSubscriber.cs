using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using SystemEx;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Client
{
    internal class WampSubscriber<TMessage> : IWampSubscriber<TMessage>, IWampTopicSubscriptionProxy,
        IWampSubscriberError<TMessage>
    {
        private readonly WampRequestIdMapper<SubscribeRequest> mPendingSubscriptions =
            new WampRequestIdMapper<SubscribeRequest>();

        private readonly WampRequestIdMapper<UnsubscribeRequest> mPendingUnsubscriptions =
            new WampRequestIdMapper<UnsubscribeRequest>();

        private readonly IWampServerProxy mProxy;
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly IWampClientConnectionMonitor mMonitor;

        private readonly ConcurrentDictionary<long, Subscription> mSubscriptionIdToSubscription =
            new ConcurrentDictionary<long, Subscription>();

        public WampSubscriber(IWampServerProxy proxy,
                              IWampFormatter<TMessage> formatter,
                              IWampClientConnectionMonitor monitor)
        {
            mProxy = proxy;
            mFormatter = formatter;
            mMonitor = monitor;

            monitor.ConnectionBroken += OnConnectionBroken;
            monitor.ConnectionError += OnConnectionError;
        }

        public Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri)
        {
            SubscribeRequest request = new SubscribeRequest(mFormatter, subscriber, options, topicUri);
            long requestId = mPendingSubscriptions.Add(request);
            request.RequestId = requestId;

            mProxy.Subscribe(requestId, options, topicUri);

            return request.Task;
        }

        private Task Unsubscribe(long subscriptionId)
        {
            UnsubscribeRequest request = new UnsubscribeRequest(mFormatter, subscriptionId);
            long requestId = mPendingUnsubscriptions.Add(request);
            request.RequestId = requestId;

            try
            {
                mProxy.Unsubscribe(requestId, subscriptionId);
            }
            catch (Exception exception)
            {
                UnsubscribeRequest removedRequest;
                mPendingUnsubscriptions.TryRemove(requestId, out removedRequest);
                request.SetException(exception);
            }

            return request.Task;
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            SubscribeRequest request;
            
            if (mPendingSubscriptions.TryRemove(requestId, out request))
            {
                IAsyncDisposable disposable = 
                    new UnsubscribeDisposable(this, subscriptionId);

                mSubscriptionIdToSubscription[subscriptionId] =
                    new Subscription(subscriptionId,
                                     request.Subscriber,
                                     request.Options,
                                     request.TopicUri,
                                     disposable);

                request.Complete(disposable);
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
            SubscribeRequest request;

            if (mPendingSubscriptions.TryRemove(requestId, out request))
            {
                request.Error(details, error);
            }
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            SubscribeRequest request;

            if (mPendingSubscriptions.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments);
            }
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            SubscribeRequest request;

            if (mPendingSubscriptions.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void UnsubscribeError(long requestId, TMessage details, string error)
        {
            UnsubscribeRequest request;

            if (mPendingUnsubscriptions.TryRemove(requestId, out request))
            {
                request.Error(details, error);
            }
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            UnsubscribeRequest request;

            if (mPendingUnsubscriptions.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments);
            }
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            UnsubscribeRequest request;

            if (mPendingUnsubscriptions.TryRemove(requestId, out request))
            {
                request.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details)
        {
            InnerEvent(subscriptionId, 
                subscriber => subscriber.Event(Formatter, publicationId, details));
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments)
        {
            InnerEvent(subscriptionId,
                       subscriber => subscriber.Event(Formatter,
                                                      publicationId,
                                                      details,
                                                      arguments));
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            InnerEvent(subscriptionId,
                       subscriber => subscriber.Event(Formatter,
                                                      publicationId,
                                                      details,
                                                      arguments,
                                                      argumentsKeywords));
        }

        private void InnerEvent(long subscriptionId, Action<IWampRawTopicClientSubscriber> action)
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

        public void OnConnectionError(object sender, WampConnectionErrorEventArgs eventArgs)
        {
            Exception exception = eventArgs.Exception;

            mPendingSubscriptions.ConnectionError(exception);
            mPendingUnsubscriptions.ConnectionError(exception);
            
            Cleanup();
        }

        public void OnConnectionBroken(object sender, WampSessionCloseEventArgs eventArgs)
        {
            // TODO: clean up topics
            mPendingSubscriptions.ConnectionClosed(eventArgs);
            mPendingUnsubscriptions.ConnectionClosed(eventArgs);
            Cleanup();
        }

        private void Cleanup()
        {
            mSubscriptionIdToSubscription.Clear();
        }

        public void Dispose()
        {
            foreach (var idToSubscription in mSubscriptionIdToSubscription)
            {
                idToSubscription.Value.Dispose();
            }

            mSubscriptionIdToSubscription.Clear();
        }

        private class BaseSubscription
        {
            private readonly IWampRawTopicClientSubscriber mSubscriber;
            private readonly SubscribeOptions mOptions;
            private readonly string mTopicUri;

            public BaseSubscription(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri)
            {
                mSubscriber = subscriber;
                mOptions = options;
                mTopicUri = topicUri;
            }

            public IWampRawTopicClientSubscriber Subscriber
            {
                get
                {
                    return mSubscriber;
                }
            }

            public SubscribeOptions Options
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

        private class SubscribeRequest : BaseSubscription, IWampPendingRequest
        {
            private readonly WampPendingRequest<TMessage, IAsyncDisposable> mPendingRequest;

            public SubscribeRequest(IWampFormatter<TMessage> formatter, IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri) : 
                base(subscriber, options, topicUri)
            {
                mPendingRequest = new WampPendingRequest<TMessage, IAsyncDisposable>(formatter);
            }

            public long RequestId
            {
                get { return mPendingRequest.RequestId; }
                set { mPendingRequest.RequestId = value; }
            }

            public void SetException(Exception exception)
            {
                mPendingRequest.SetException(exception);
            }

            public void Error(TMessage details, string error)
            {
                mPendingRequest.Error(details, error);
            }

            public void Error(TMessage details, string error, TMessage[] arguments)
            {
                mPendingRequest.Error(details, error, arguments);
            }

            public void Error(TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
            {
                mPendingRequest.Error(details, error, arguments, argumentsKeywords);
            }

            public void Complete(IAsyncDisposable result)
            {
                mPendingRequest.Complete(result);
            }

            public Task<IAsyncDisposable> Task
            {
                get { return mPendingRequest.Task; }
            }
        }

        private class Subscription : BaseSubscription, IDisposable
        {
            private readonly long mSubscriptionId;
            private readonly IAsyncDisposable mDisposable;

            public Subscription(long subscriptionId, IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri, IAsyncDisposable disposable) : 
                base(subscriber, options, topicUri)
            {
                mSubscriptionId = subscriptionId;
                mDisposable = disposable;
            }

            public long SubscriptionId
            {
                get
                {
                    return mSubscriptionId;
                }
            }

            public void Dispose()
            {
                mDisposable.DisposeAsync();
            }
        }

        private class UnsubscribeRequest : WampPendingRequest<TMessage>
        {
            private readonly long mSubscriptionId;

            public UnsubscribeRequest(IWampFormatter<TMessage> formatter, long subscriptionId) : base(formatter)
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

        private class UnsubscribeDisposable : IAsyncDisposable
        {
            private readonly WampSubscriber<TMessage> mParent;
            private readonly long mSubscriptionId;

            public UnsubscribeDisposable(WampSubscriber<TMessage> parent, long subscriptionId)
            {
                mParent = parent;
                mSubscriptionId = subscriptionId;
            }

            public Task DisposeAsync()
            {
                return mParent.Unsubscribe(mSubscriptionId);
            }
        }
    }
}