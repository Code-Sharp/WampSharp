using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Handles message of a WAMP2 broker.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <remarks>These messages are part of the WAMP2 specification.</remarks>
    public interface IWampBroker<TMessage>
    {
        /// <summary>
        /// Occurs when a PUBLISH message is arrived.
        /// </summary>
        /// <param name="publisher">The publisher that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to publish this message to.</param>
        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampPublisher publisher, long requestId, PublishOptions options, string topicUri);

        /// <summary>
        /// Occurs when a PUBLISH message is arrived.
        /// </summary>
        /// <param name="publisher">The publisher that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to publish this message to.</param>
        /// <param name="arguments">The arguments to publish.</param>
        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter]IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments);

        /// <summary>
        /// Occurs when a PUBLISH message is arrived.
        /// </summary>
        /// <param name="publisher">The publisher that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to publish this message to.</param>
        /// <param name="arguments">The arguments to publish.</param>
        /// <param name="argumentKeywords">Additional argument keywords to publish.</param>
        [WampHandler(WampMessageType.v2Publish)]
        void Publish([WampProxyParameter] IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords);

        /// <summary>
        /// Occurs when a SUBSCRIBE message is arrived.
        /// </summary>
        /// <param name="subscriber">The subscriber that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to subscribe to.</param>
        [WampHandler(WampMessageType.v2Subscribe)]
        void Subscribe([WampProxyParameter] IWampSubscriber subscriber, long requestId, SubscribeOptions options, string topicUri);

        /// <summary>
        /// Occurs when a UNSUBSCRIBE message is arrived.
        /// </summary>
        /// <param name="subscriber">The subscriber that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="subscriptionId">The request subscription id to remove.</param>
        [WampHandler(WampMessageType.v2Unsubscribe)]
        void Unsubscribe([WampProxyParameter]IWampSubscriber subscriber, long requestId, long subscriptionId);
    }
}