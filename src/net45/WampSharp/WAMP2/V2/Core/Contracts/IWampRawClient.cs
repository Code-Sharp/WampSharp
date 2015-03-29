using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents a proxy to a client that can send raw messages.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampRawClient<TMessage>
    {
        /// <summary>
        /// Sends a raw message to current client.
        /// </summary>
        /// <param name="message">The message to send.</param>
        [WampRawHandler]
        void Message(WampMessage<object> message);
    }
}