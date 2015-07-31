using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// Contains the pub/sub methods of a WAMP client.
    /// </summary>
    public interface IWampPubSubClient<TMessage>
    {
        /// <summary>
        /// Occurs when a new event is published to topic 
        /// the client is subscribed to.
        /// </summary>
        /// <param name="topicUri">The topic uri the current event is published to.</param>
        /// <param name="event">The current event.</param>
        [WampHandler(WampMessageType.v1Event)]
        void Event(string topicUri, TMessage @event); 
    }

    /// <summary>
    /// An object version of <see cref="IWampPubSubClient{TMessage}"/>
    /// </summary>
    public interface IWampPubSubClient : IWampPubSubClient<object>
    {
    }
}