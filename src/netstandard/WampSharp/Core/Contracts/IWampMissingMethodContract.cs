using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    /// <summary>
    /// Handles messages that are not handled by other implemented interfaces
    /// of a given handler.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampMissingMethodContract<TMessage>
    {
        /// <summary>
        /// Occurs when a message isn't handled by any other interface
        /// method of the handler.
        /// </summary>
        /// <param name="rawMessage">The message.</param>
        [WampRawHandler]
        void Missing(WampMessage<TMessage> rawMessage);
    }

    /// <summary>
    /// Handles messages that are not handled by other implemented interfaces
    /// of a given handler.
    /// </summary>
    public interface IWampMissingMethodContract<TMessage, TClient>
    {
        /// <summary>
        /// Occurs when a message isn't handled by any other interface
        /// method of the handler.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="rawMessage">The message.</param>
        [WampRawHandler]
        void Missing([WampProxyParameter] TClient client, WampMessage<TMessage> rawMessage);
    }
}