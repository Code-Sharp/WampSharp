using System;
using System.Collections.Concurrent;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public class WampTopicContainerProxy<TMessage> : IWampTopicContainerProxy,
        IWampSubscriber<TMessage>, IWampPublisher<TMessage>
    {
        private readonly IWampServerProxy mProxy;
        private readonly WampSubscriber<TMessage> mSubscriber;
        private readonly WampPublisher<TMessage> mPublisher;
        
        private readonly ConcurrentDictionary<string, WampTopicProxy<TMessage>> mTopicUriToProxy =
            new ConcurrentDictionary<string, WampTopicProxy<TMessage>>();

        public WampTopicContainerProxy(IWampServerProxy proxy)
        {
            mProxy = proxy;
            mSubscriber = new WampSubscriber<TMessage>(proxy);
            mPublisher = new WampPublisher<TMessage>(proxy);
        }

        public IWampTopicProxy GetTopic(string topicUri)
        {
            return mTopicUriToProxy.GetOrAdd(topicUri, uri => CreateTopicUri(uri));
        }

        private WampTopicProxy<TMessage> CreateTopicUri(string topicUri)
        {
            WampTopicProxy<TMessage> result =
                new WampTopicProxy<TMessage>
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
                WampTopicProxy<TMessage> value;
                mParent.mTopicUriToProxy.TryRemove(mTopicUri, out value);
            }
        }
    }
}