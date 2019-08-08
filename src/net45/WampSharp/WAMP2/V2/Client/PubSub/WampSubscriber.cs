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
        private readonly IWampClientConnectionMonitor mMonitor;

        private readonly SwapDictionary<long, SwapCollection<Subscription>> mSubscriptionIdToSubscriptions =
            new SwapDictionary<long, SwapCollection<Subscription>>();

        private readonly object mLock = new object();

        public WampSubscriber(IWampServerProxy proxy,
                              IWampFormatter<TMessage> formatter,
                              IWampClientConnectionMonitor monitor)
        {
            mProxy = proxy;
            Formatter = formatter;
            mMonitor = monitor;

            monitor.ConnectionBroken += OnConnectionBroken;
            monitor.ConnectionError += OnConnectionError;
        }

        private bool IsConnected => mMonitor.IsConnected;

        public Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri)
        {
            if (!IsConnected)
            {
                throw new WampSessionNotEstablishedException();
            }

            SubscribeRequest request = new SubscribeRequest(Formatter, subscriber, options, topicUri);
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

                long subscriptionId = subscription.SubscriptionId;

                if (!mSubscriptionIdToSubscriptions.TryGetValue(subscriptionId, out SwapCollection<Subscription> subscriptions))
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

            UnsubscribeRequest request = new UnsubscribeRequest(Formatter, subscriptionId);
            long requestId = mPendingUnsubscriptions.Add(request);
            request.RequestId = requestId;

            try
            {
                mProxy.Unsubscribe(requestId, subscriptionId);
            }
            catch (Exception exception)
            {
                mPendingUnsubscriptions.TryRemove(requestId, out UnsubscribeRequest removedRequest);
                request.SetException(exception);
            }

            return request.Task;
        }

        public void Subscribed(long requestId, long subscriptionId)
        {

            if (mPendingSubscriptions.TryRemove(requestId, out SubscribeRequest request))
            {
                Subscription subscription =
                    new Subscription(subscriptionId,
                    request.Subscriber,
                    request.Options,
                    request.TopicUri);

                UnsubscribeDisposable disposable =
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

            if (mPendingUnsubscriptions.TryRemove(requestId, out UnsubscribeRequest request))
            {
                request.Complete();
            }
        }

        public void SubscribeError(long requestId, TMessage details, string error)
        {

            if (mPendingSubscriptions.TryRemove(requestId, out SubscribeRequest request))
            {
                request.Error(details, error);
            }
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {

            if (mPendingSubscriptions.TryRemove(requestId, out SubscribeRequest request))
            {
                request.Error(details, error, arguments);
            }
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {

            if (mPendingSubscriptions.TryRemove(requestId, out SubscribeRequest request))
            {
                request.Error(details, error, arguments, argumentsKeywords);
            }
        }

        public void UnsubscribeError(long requestId, TMessage details, string error)
        {

            if (mPendingUnsubscriptions.TryRemove(requestId, out UnsubscribeRequest request))
            {
                request.Error(details, error);
            }
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {

            if (mPendingUnsubscriptions.TryRemove(requestId, out UnsubscribeRequest request))
            {
                request.Error(details, error, arguments);
            }
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {

            if (mPendingUnsubscriptions.TryRemove(requestId, out UnsubscribeRequest request))
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

            if (mSubscriptionIdToSubscriptions.TryGetValue(subscriptionId, out SwapCollection<Subscription> subscriptions))
            {
                foreach (Subscription subscription in subscriptions)
                {
                    EventDetails modifiedDetails = new EventDetails(details);
                    modifiedDetails.Topic = modifiedDetails.Topic ?? subscription.TopicUri;
                    action(subscription.Subscriber, modifiedDetails);
                }
            }
        }

        private IWampFormatter<TMessage> Formatter { get; }

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
            private readonly string mTopicUri;

            public BaseSubscription(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri)
            {
                Subscriber = subscriber;
                Options = options;
                mTopicUri = topicUri;
            }

            public IWampRawTopicClientSubscriber Subscriber { get; }

            public SubscribeOptions Options { get; }

            public string TopicUri => mTopicUri;
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
                get => mPendingRequest.RequestId;
                set => mPendingRequest.RequestId = value;
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

            public Task<IAsyncDisposable> Task => mPendingRequest.Task;
        }

        private class Subscription : BaseSubscription
        {
            public Subscription(long subscriptionId, IWampRawTopicClientSubscriber subscriber, SubscribeOptions options, string topicUri) : 
                base(subscriber, options, topicUri)
            {
                SubscriptionId = subscriptionId;
            }

            public long SubscriptionId { get; }
        }

        private class UnsubscribeRequest : WampPendingRequest<TMessage>
        {
            public UnsubscribeRequest(IWampFormatter<TMessage> formatter, long subscriptionId) : base(formatter)
            {
                SubscriptionId = subscriptionId;
            }

            public long SubscriptionId { get; }
        }

        private class UnsubscribeDisposable : ITaskAsyncDisposable, IAsyncDisposable
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

            ValueTask IAsyncDisposable.DisposeAsync()
            {
                Task result = this.DisposeAsync();
                return new ValueTask(result);
            }
        }
    }
}