using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    /// <summary>
    /// Handles outgoing <see cref="WampMessage{TMessage}"/>s.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampOutgoingMessageHandler<TMessage>
    {
        /// <summary>
        /// Handles a given outgoing <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <param name="message">The given message.</param>
        void Handle(WampMessage<TMessage> message);
    }
}