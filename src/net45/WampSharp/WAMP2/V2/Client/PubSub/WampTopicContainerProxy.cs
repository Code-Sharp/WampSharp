using System;
using System.Collections.Concurrent;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampTopicContainerProxy<TMessage> : IWampTopicContainerProxy,
        IWampSubscriber<TMessage>, IWampPublisher<TMessage>,
        IWampSubscriberError<TMessage>, IWampPublisherError<TMessage>
    {
        private readonly IWampServerProxy mProxy;

        private readonly WampSubscriber<TMessage> mSubscriber;

        private readonly WampPublisher<TMessage> mPublisher;

        private readonly ConcurrentDictionary<string, WampTopicProxy> mTopicUriToProxy =
            new ConcurrentDictionary<string, WampTopicProxy>();

        public WampTopicContainerProxy(IWampServerProxy proxy, IWampFormatter<TMessage> formatter)
        {
            mProxy = proxy;
            mSubscriber = new WampSubscriber<TMessage>(proxy, formatter);
            mPublisher = new WampPublisher<TMessage>(proxy);
        }

        public IWampTopicProxy GetTopicByUri(string topicUri)
        {
            return mTopicUriToProxy.GetOrAdd(topicUri, uri => CreateTopicUri(uri));
        }

        private WampTopicProxy CreateTopicUri(string topicUri)
        {
            WampTopicProxy result =
                new WampTopicProxy
                    (topicUri, 
                    mSubscriber, 
                    mPublisher,
                    new ConatinerDisposable(this, topicUri));

            return result;
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            mSubscriber.Subscribed(requestId, subscriptionId);
        }

        public void Unsubscribed(long requestId, long subscriptionId)
        {
            mSubscriber.Unsubscribed(requestId, subscriptionId);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details)
        {
            mSubscriber.Event(subscriptionId, publicationId, details);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments)
        {
            mSubscriber.Event(subscriptionId, publicationId, details, arguments);
        }

        public void Event(long subscriptionId, long publicationId, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mSubscriber.Event(subscriptionId, publicationId, details, arguments, argumentsKeywords);
        }

        public void Published(long requestId, long publicationId)
        {
            mPublisher.Published(requestId, publicationId);
        }

        public void PublishError(long requestId, TMessage details, string error)
        {
            mPublisher.PublishError(requestId, details, error);
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mPublisher.PublishError(requestId, details, error, arguments);
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mPublisher.PublishError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void SubscribeError(long requestId, TMessage details, string error)
        {
            mSubscriber.SubscribeError(requestId, details, error);
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mSubscriber.SubscribeError(requestId, details, error, arguments);
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mSubscriber.SubscribeError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void UnsubscribeError(long requestId, TMessage details, string error)
        {
            mSubscriber.UnsubscribeError(requestId, details, error);
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mSubscriber.UnsubscribeError(requestId, details, error, arguments);
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mSubscriber.UnsubscribeError(requestId, details, error, arguments, argumentsKeywords);
        }

        private class ConatinerDisposable : IDisposable
        {
            private readonly WampTopicContainerProxy<TMessage> mParent;
            private readonly string mTopicUri;

            public ConatinerDisposable(WampTopicContainerProxy<TMessage> parent,
                                       string topicUri)
            {
                mParent = parent;
                mTopicUri = topicUri;
            }

            public void Dispose()
            {
                WampTopicProxy value;
                mParent.mTopicUriToProxy.TryRemove(mTopicUri, out value);
            }
        }
    }
}