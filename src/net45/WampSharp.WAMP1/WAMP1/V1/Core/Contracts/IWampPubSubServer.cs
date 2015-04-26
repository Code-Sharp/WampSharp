using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// Contains the pub/sub methods of a WAMP server.
    /// </summary>
    public interface IWampPubSubServer<TMessage>
    {
        /// <summary>
        /// Occurs when a client subscribes to a topic uri.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="topicUri">The topic uri the client subscribed to.</param>
        [WampHandler(WampMessageType.v1Subscribe)]
        void Subscribe([WampProxyParameter]IWampClient client, string topicUri);

        /// <summary>
        /// Occurs when a client unsubscribes from a topic uri.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="topicUri">The topic uri the client unsubscribed from.</param>
        [WampHandler(WampMessageType.v1Unsubscribe)]
        void Unsubscribe([WampProxyParameter]IWampClient client, string topicUri);

        /// <summary>
        /// Occurs when a client publishes an event to a topic uri.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="topicUri">The topic uri the client published to.</param>
        /// <param name="event">The event the client published.</param>
        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event);

        /// <summary>
        /// Occurs when a client publishes an event to a topic uri.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="topicUri">The topic uri the client published to.</param>
        /// <param name="event">The event the client published.</param>
        /// <param name="excludeMe">A value indicating whether to exclude the client from
        /// the recipients of the message.</param>
        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event, bool excludeMe);

        /// <summary>
        /// Occurs when a client publishes an event to a topic uri.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="topicUri">The topic uri the client published to.</param>
        /// <param name="event">The event the client published.</param>
        /// <param name="exclude">A collection of session ids of clients being excluded from the
        /// recipients of the event.</param>
        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event, string[] exclude);

        /// <summary>
        /// Occurs when a client publishes an event to a topic uri.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="topicUri">The topic uri the client published to.</param>
        /// <param name="event">The event the client published.</param>
        /// <param name="exclude">A collection of session ids of clients being excluded from the
        /// recipients of the event.</param>
        /// <param name="eligible">A collection of session ids of the only clients that should not be exluded from the
        /// recipients of the event.</param>
        [WampHandler(WampMessageType.v1Publish)]
        void Publish([WampProxyParameter]IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible);
    }
}