using WampSharp.Core.Proxy;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Builds an <see cref="IWampOutgoingMessageHandler"/>
    /// for a given connection.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampOutgoingMessageHandlerBuilder<TMessage>
    {
        /// <summary>
        /// Builds an <see cref="IWampOutgoingMessageHandler"/>
        /// for the given <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <returns>The built <see cref="IWampOutgoingMessageHandler"/></returns>
        IWampOutgoingMessageHandler Build(IWampConnection<TMessage> connection);
    }
}