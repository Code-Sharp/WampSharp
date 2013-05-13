using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Builds an <see cref="IWampOutgoingMessageHandler{TMessage}"/>
    /// for a given connection.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampOutgoingMessageHandlerBuilder<TMessage>
    {
        /// <summary>
        /// Builds an <see cref="IWampOutgoingMessageHandler{TMessage}"/>
        /// for the given <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <returns>The built <see cref="IWampOutgoingMessageHandler{TMessage}"/></returns>
        IWampOutgoingMessageHandler<TMessage> Build(IWampConnection<TMessage> connection);
    }
}