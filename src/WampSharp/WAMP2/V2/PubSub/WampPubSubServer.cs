using System;
using System.Collections.Generic;
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
            bool acknowledge = ShouldAcknowledge(options);

            try
            {
                long publicationId = mRawTopicContainer.Publish(options, topicUri);
                SendPublishAckIfNeeded(publisher, requestId, publicationId, acknowledge);
            }
            catch (Exception ex)
            {
                PublishErrorIfNeeded(publisher, requestId, acknowledge, ex);
            }
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri,
                            TMessage[] arguments)
        {
            bool acknowledge = ShouldAcknowledge(options);

            try
            {
                long publicationId = mRawTopicContainer.Publish(options, topicUri, arguments);
                SendPublishAckIfNeeded(publisher, requestId, publicationId, acknowledge);
            }
            catch (Exception ex)
            {
                PublishErrorIfNeeded(publisher, requestId, acknowledge, ex);
            }
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri,
                            TMessage[] arguments,
                            TMessage argumentKeywords)
        {
            bool acknowledge = ShouldAcknowledge(options);

            try
            {
                long publicationId = mRawTopicContainer.Publish(options, topicUri, arguments, argumentKeywords);
                SendPublishAckIfNeeded(publisher, requestId, publicationId, acknowledge);
            }
            catch (Exception ex)
            {
                PublishErrorIfNeeded(publisher, requestId, acknowledge, ex);
            }
        }

        public void Subscribe(IWampSubscriber subscriber, long requestId, TMessage options, string topicUri)
        {
            try
            {
                SubscribeRequest<TMessage> subscribeRequest = 
                    new SubscribeRequest<TMessage>(subscriber, requestId);
                
                mRawTopicContainer.Subscribe(subscribeRequest, options, topicUri);
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
                UnsubscribeRequest<TMessage> unsubscribeRequest =
                    new UnsubscribeRequest<TMessage>(subscriber, requestId, subscriptionId);

                mRawTopicContainer.Unsubscribe(unsubscribeRequest, subscriptionId);
            }
            catch (Exception ex)
            {
                subscriber.UnsubscribeError(requestId, ex);
            }
        }

        private void SendPublishAckIfNeeded(IWampPublisher publisher, long requestId, long publicationId, bool acknowledge)
        {
            if (acknowledge)
            {
                publisher.Published(requestId, publicationId);
            }
        }

        private void PublishErrorIfNeeded(IWampPublisher publisher, long requestId, bool acknowledge, Exception ex)
        {
            if (acknowledge)
            {
                publisher.PublishError(requestId, ex);
            }
        }

        private bool ShouldAcknowledge(TMessage options)
        {
            // I don't want to create an object that has a (lower case)
            // acknowledge property, and I don't want to put any attributes
            // from the external libraries. I hope they support this.
            // The serializing/deserializing options issue should be rethought of
            // later.
            var castedOptions =
                DeserializeOptions(options, new {acknowledge = default(bool)});

            return castedOptions.acknowledge;
        }

        private TSample DeserializeOptions<TSample>(TMessage options, TSample sample)
        {
            return mBinding.Formatter.Deserialize<TSample>(options);
        }
    }
}