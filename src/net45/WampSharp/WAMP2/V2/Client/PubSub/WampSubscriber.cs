using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemEx;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
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

        private readonly SwapDictionary<long, SwapCollection<Subscription>> mSubscriptionIdToSubscriptions =
            new SwapDictionary<long, SwapCollection<Subscription>>();

        private readonly object mLock = new object();

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

        private bool IsConnected
        {
            get
            {
                return mMonitor.IsConnected;
            }
        }

        public Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            SubscribeRequest request = new SubscribeRequest(mFormatter, subscriber, options, topicUri);
            long requestId = mPendingSubscriptions.Add(request);
            request.RequestId = requestId;

            mProxy.Subscribe(requestId, options, topicUri);

            return request.Task;
        }

        private Task Unsubscribe(Subscription subscription)
        {
            TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
            Task result = completionSource.Task;

            lock (mLock)
            {
                SwapCollection<Subscription> subscriptions;

                long subscriptionId = subscription.SubscriptionId;

                if (!mSubscriptionIdToSubscriptions.TryGetValue(subscriptionId, out subscriptions))
                {
                    completionSource.SetException(new Exception("Unknown subscription: " + subscriptionId));
                }
                else
                {
                    subscriptions.Remove(subscription);

                    if (subscriptions.Count != 0)
                    {
                        completionSource.SetResult(true);
                    }
                    else
                    {
                        mSubscriptionIdToSubscriptions.Remove(subscriptionId);
                        result = UnsubscribeExternal(subscriptionId);
                    }
                }
            }

            return result;
        }

        private Task UnsubscribeExternal(long subscriptionId)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

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
                Subscription subscription =
                    new Subscription(subscriptionId,
                    request.Subscriber,
                    request.Options,
                    request.TopicUri);

                IAsyncDisposable disposable =
                    new UnsubscribeDisposable(this, subscription);

                lock (mLock)
                {
                    mSubscriptionIdToSubscriptions.Add(subscriptionId, subscription);
                }

                request.Complete(disposable);
            }
        }

        public void Unsubscribed(long requestId)
        {
            UnsubscribeRequest request;

            if (mPendingUnsubscriptions.TryRemove(requestId, out request))
            {
                request.Complete();
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
                       details,
                       (subscriber, eventDetails) => subscriber.Event(Formatter,
                                                                      publicationId,
                                                                      eventDetails));
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments)
        {
            InnerEvent(subscriptionId,
                       details,
                       (subscriber, eventDetails) => subscriber.Event(Formatter,
                                                                      publicationId,
                                                                      eventDetails,
                                                                      arguments));
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            InnerEvent(subscriptionId,
                       details,
                       (subscriber, eventDetails) => subscriber.Event(Formatter,
                                                                      publicationId,
                                                                      eventDetails,
                                                                      arguments,
                                                                      argumentsKeywords));
        }

        private void InnerEvent(long subscriptionId, EventDetails details, Action<IWampRawTopicClientSubscriber, EventDetails> action)
        {
            SwapCollection<Subscription> subscriptions;

            if (mSubscriptionIdToSubscriptions.TryGetValue(subscriptionId, out subscriptions))
            {
                foreach (Subscription subscription in subscriptions)
                {
                    EventDetails modifiedDetails = new EventDetails(details);
                    modifiedDetails.Topic = modifiedDetails.Topic ?? subscription.TopicUri;
                    action(subscription.Subscriber, modifiedDetails);                    
                }
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
            mSubscriptionIdToSubscriptions.Clear();
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

        private class Subscription : BaseSubscription
        {
            private readonly long mSubscriptionId;

            public Subscription(long subscriptionId, IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri) : 
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
            private readonly Subscription mSubscription;

            public UnsubscribeDisposable(WampSubscriber<TMessage> parent, Subscription subscription)
            {
                mParent = parent;
                mSubscription = subscription;
            }

            public Task DisposeAsync()
            {
                return mParent.Unsubscribe(mSubscription);
            }
        }
    }
}