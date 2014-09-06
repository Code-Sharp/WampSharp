using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Binding;
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
            InnerPublish(publisher, requestId, options,
                         publishOptions => mRawTopicContainer.Publish(publishOptions, topicUri));
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri,
                            TMessage[] arguments)
        {
            InnerPublish(publisher, requestId, options,
                         publishOptions => mRawTopicContainer.Publish(publishOptions, topicUri, arguments));
        }

        public void Publish(IWampPublisher publisher, long requestId, TMessage options, string topicUri,
                            TMessage[] arguments,
                            TMessage argumentKeywords)
        {
            InnerPublish(publisher, requestId, options,
                         publishOptions => mRawTopicContainer.Publish(publishOptions, topicUri, arguments, argumentKeywords));
        }

        private void InnerPublish(IWampPublisher publisher, long requestId, TMessage options, Func<PublishOptions, long> action)
        {
            PublishOptions publishOptions = GetPublishOptions(publisher, options);

            bool acknowledge = publishOptions.Acknowledge ?? false;

            try
            {
                long publicationId = action(publishOptions);
                SendPublishAckIfNeeded(publisher, requestId, publicationId, acknowledge);
            }
            catch (WampException ex)
            {
                PublishErrorIfNeeded(publisher, requestId, acknowledge, ex);
            }
        }

        private PublishOptions GetPublishOptions(IWampPublisher publisher, TMessage options)
        {
            PublishOptionsExtended publishOptions =
                mBinding.Formatter.Deserialize<PublishOptionsExtended>(options);

            IWampClient casted = publisher as IWampClient;

            publishOptions.PublisherId = casted.Session;
            
            return publishOptions;
        }

        public void Subscribe(IWampSubscriber subscriber, long requestId, TMessage options, string topicUri)
        {
            try
            {
                SubscribeRequest<TMessage> subscribeRequest = 
                    new SubscribeRequest<TMessage>(subscriber, requestId);
                
                mRawTopicContainer.Subscribe(subscribeRequest, options, topicUri);
            }
            catch (WampException ex)
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
            catch (WampException ex)
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

        private void PublishErrorIfNeeded(IWampPublisher publisher, long requestId, bool acknowledge, WampException ex)
        {
            if (acknowledge)
            {
                publisher.PublishError(requestId, ex);
            }
        }
    }
}