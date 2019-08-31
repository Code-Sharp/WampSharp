using System;
using System.Collections.Generic;
using WampSharp.Logging;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class WampPubSubServer<TMessage> : IWampPubSubServer<TMessage>
    {
        private readonly IWampEventSerializer mEventSerializer;
        private readonly ILog mLogger;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampUriValidator mUriValidator;
        private readonly IWampRawTopicContainer<TMessage> mRawTopicContainer;

        public WampPubSubServer(IWampTopicContainer topicContainer, IWampEventSerializer eventSerializer, IWampBinding<TMessage> binding, IWampUriValidator uriValidator)
        {
            mLogger = LogProvider.GetLogger(this.GetType());
            mBinding = binding;
            mUriValidator = uriValidator;
            mEventSerializer = eventSerializer;
            mRawTopicContainer = new WampRawTopicContainer<TMessage>(topicContainer, mEventSerializer, mBinding);
        }

        public void Publish(IWampPublisher publisher,
            long requestId,
            PublishOptions options,
            string topicUri)
        {
            InnerPublish(publisher, topicUri, requestId, options, publishOptions => mRawTopicContainer.Publish(publishOptions, topicUri));
        }

        public void Publish(IWampPublisher publisher,
            long requestId,
            PublishOptions options,
            string topicUri,
            TMessage[] arguments)
        {
            InnerPublish(publisher, topicUri, requestId, options, publishOptions => mRawTopicContainer.Publish(publishOptions, topicUri, arguments));
        }

        public void Publish(IWampPublisher publisher,
            long requestId,
            PublishOptions options,
            string topicUri,
            TMessage[] arguments,
            IDictionary<string, TMessage> argumentKeywords)
        {
            InnerPublish(publisher, topicUri, requestId, options, publishOptions => mRawTopicContainer.Publish(publishOptions, topicUri, arguments, argumentKeywords));
        }

        private void InnerPublish(IWampPublisher publisher, string topicUri, long requestId, PublishOptions options,
                                  Func<PublishOptions, long> action)
        {
            PublishOptions publishOptions = GetPublishOptions(publisher, topicUri, options);

            bool acknowledge = publishOptions.Acknowledge ?? false;

            try
            {
                ValidatePublishUri(topicUri);

                long publicationId = action(publishOptions);

                SendPublishAckIfNeeded(publisher, requestId, publicationId, acknowledge);
            }
            catch (WampException ex)
            {
                mLogger.ErrorFormat(ex,
                                    "Failed publishing to topic '{TopicUri}'. Publication request id: {RequestId}",
                                    topicUri, requestId);

                PublishErrorIfNeeded(publisher, requestId, acknowledge, ex);
            }
        }

        private PublishOptions GetPublishOptions(IWampPublisher publisher, string topicUri, PublishOptions options)
        {
            IWampClientProxy casted = publisher as IWampClientProxy;

            PublishOptionsExtended result = new PublishOptionsExtended(options);

            result.PublisherId = casted.Session;

            WelcomeDetails welcomeDetails = casted.WelcomeDetails;

            result.AuthenticationId = welcomeDetails.AuthenticationId;
            result.AuthenticationRole = welcomeDetails.AuthenticationRole;

            result.TopicUri = topicUri;

            return result;
        }

        public void Subscribe(IWampSubscriber subscriber, long requestId, SubscribeOptions options, string topicUri)
        {
            try
            {
                options = options.WithDefaults();

                ValidateSubscribeUri(topicUri, options.Match);

                SubscribeRequest<TMessage> subscribeRequest = 
                    new SubscribeRequest<TMessage>(subscriber, requestId);

                mRawTopicContainer.Subscribe(subscribeRequest, options, topicUri);
            }
            catch (WampException ex)
            {
                mLogger.ErrorFormat(ex,
                                    "Failed subscribing to topic '{TopicUri}'. Subscription request id: {RequestId}",
                                    topicUri, requestId);

                subscriber.SubscribeError(requestId, ex);
            }
        }

        private void ValidateSubscribeUri(string topicUri, string match)
        {
            if (!mUriValidator.IsValid(topicUri, match))
            {
                throw new WampException(WampErrors.InvalidUri,
                                        $"subscribe for invalid topic URI '{topicUri}'");
            }
        }

        private void ValidatePublishUri(string topicUri)
        {
            if (!mUriValidator.IsValid(topicUri))
            {
                WampException exception =
                    new WampException(WampErrors.InvalidUri,
                                      $"publish with invalid topic URI '{topicUri}'");

                throw exception;
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