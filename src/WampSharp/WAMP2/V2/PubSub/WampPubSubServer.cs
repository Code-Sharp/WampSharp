using System;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    public class WampPubSubServer<TMessage> : IWampPubSubServer<TMessage>
    {
        private readonly IWampEventSerializer<TMessage> mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampRawTopicContainer<TMessage> mRawTopicContainer;

        public WampPubSubServer(IWampTopicContainer topicContainer, IWampEventSerializer<TMessage> eventSerializer, IWampBinding<TMessage> binding)
        {
            mBinding = binding;
            mEventSerializer = eventSerializer;
            mRawTopicContainer = new WampRawTopicContainer<TMessage>(topicContainer, mEventSerializer, mBinding);
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri)
        {
            try
            {
                long publicationId = mRawTopicContainer.Publish(options, topicUri);
                publisher.Published(requestId, publicationId);
            }
            catch (Exception ex)
            {
                publisher.PublishError(requestId, ex);
            }
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri,
                            TMessage[] arguments)
        {
            try
            {
                long publicationId = mRawTopicContainer.Publish(options, topicUri, arguments);
                publisher.Published(requestId, publicationId);
            }
            catch (Exception ex)
            {
                publisher.PublishError(requestId, ex, arguments.Cast<object>().ToArray());
            }
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri,
                            TMessage[] arguments,
                            TMessage argumentKeywords)
        {
            try
            {
                long publicationId = mRawTopicContainer.Publish(options, topicUri, arguments, argumentKeywords);
                publisher.Published(requestId, publicationId);
            }
            catch (Exception ex)
            {
                publisher.PublishError(requestId, ex, arguments.Cast<object>().ToArray(), argumentKeywords);
            }
        }

        public void Subscribe(IWampSubscriber subscriber, long requestId, TMessage options, string topicUri)
        {
            try
            {
                long subscriptionId =
                    mRawTopicContainer.Subscribe(subscriber, options, topicUri);

                subscriber.Subscribed(requestId, subscriptionId);
            }
            catch (Exception ex)
            {
                subscriber.SubscribeError(requestId, ex);
            }
        }

        public void Unsubscribe(IWampSubscriber subscriber, long requestId, long subscriptionId)
        {
            try
            {
                mRawTopicContainer.Unsubscribe(subscriber, subscriptionId);

                subscriber.Unsubscribed(requestId, subscriptionId);
            }
            catch (Exception ex)
            {
                subscriber.UnsubscribeError(requestId, ex);
            }
        }
    }
}