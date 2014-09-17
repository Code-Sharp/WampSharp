using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents a proxy to a WAMP2 broker.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <remarks>These messages are part of the WAMP2 specification.</remarks>
    public interface IWampBrokerProxy<TMessage>
    {
        /// <summary>
        /// Sends a PUBLISH message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to publish this message to.</param>
        [WampHandler(WampMessageType.v2Publish)]
        void Publish(long requestId, PublishOptions options, string topicUri);

        /// <summary>
        /// Sends a PUBLISH message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to publish this message to.</param>
        /// <param name="arguments">The arguments to publish.</param>
        [WampHandler(WampMessageType.v2Publish)]
        void Publish(long requestId, PublishOptions options, string topicUri, TMessage[] arguments);

        /// <summary>
        /// Sends a PUBLISH message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to publish this message to.</param>
        /// <param name="arguments">The arguments to publish.</param>
        /// <param name="argumentKeywords">Additional argument keywords to publish.</param>
        [WampHandler(WampMessageType.v2Publish)]
        void Publish(long requestId, PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords);

        /// <summary>
        /// Sends a SUBSCRIBE message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="topicUri">The uri of the topic to subscribe to.</param>
        [WampHandler(WampMessageType.v2Subscribe)]
        void Subscribe(long requestId, SubscribeOptions options, string topicUri);

        /// <summary>
        /// Sends an UNSUBSCRIBE message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="subscriptionId">The request subscription id to remove.</param>
        [WampHandler(WampMessageType.v2Unsubscribe)]
        void Unsubscribe(long requestId, long subscriptionId);
    }
}