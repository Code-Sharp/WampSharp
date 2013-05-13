using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    /// <summary>
    /// Handles incoming <see cref="WampMessage{TMessage}"/>s receieved from clients.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    /// <remarks>
    /// This should probably dispatch the given message to its corresponding
    /// method.
    /// </remarks>
    public interface IWampIncomingMessageHandler<TMessage, TClient>
    {
        /// <summary>
        /// Handles a given incoming message from a given client.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="message">The message.</param>
        void HandleMessage(TClient client, WampMessage<TMessage> message);
    }

    /// <summary>
    /// Handles <see cref="WampMessage{TMessage}"/>s.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampIncomingMessageHandler<TMessage>
    {
        /// <summary>
        /// Handles a given incoming message.
        /// </summary>
        /// <param name="message">The message.</param>
        void HandleMessage(WampMessage<TMessage> message);         
    }
}