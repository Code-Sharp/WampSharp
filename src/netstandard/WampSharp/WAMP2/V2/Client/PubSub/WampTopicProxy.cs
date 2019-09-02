using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    internal class WampTopicProxy : IWampTopicProxy
    {
        private readonly IWampTopicSubscriptionProxy mSubscriber;
        private readonly IWampTopicPublicationProxy mPublisher;

        public WampTopicProxy(string topicUri,
                              IWampTopicSubscriptionProxy subscriber,
                              IWampTopicPublicationProxy publisher)
        {
            TopicUri = topicUri;
            mSubscriber = subscriber;
            mPublisher = publisher;
        }

        public string TopicUri { get; }

        public Task<long?> Publish(PublishOptions options)
        {
            return mPublisher.Publish(this.TopicUri, options);
        }

        public Task<long?> Publish(PublishOptions options, object[] arguments)
        {
            return mPublisher.Publish(this.TopicUri, options, arguments);
        }

        public Task<long?> Publish(PublishOptions options, object[] arguments, IDictionary<string, object> argumentKeywords)
        {
            return mPublisher.Publish(this.TopicUri, options, arguments, argumentKeywords);
        }

        public Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options)
        {
            Task<IAsyncDisposable> task =
                mSubscriber.Subscribe(subscriber, options, this.TopicUri);

            return task;
        }
    }
}