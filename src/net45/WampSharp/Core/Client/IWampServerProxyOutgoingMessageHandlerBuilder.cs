using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// Creates a <see cref="IWampOutgoingMessageHandler"/> that will
    /// send messages to a WAMP server.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    public interface IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient>
    {
        /// <summary>
        /// Builds a <see cref="IWampOutgoingMessageHandler"/> that
        /// will send messages using the given <paramref name="connection"/>
        /// and get callbacks through the given <paramref name="client"/>.
        /// </summary>
        /// <param name="client">The client to receive callbacks from server through.</param>
        /// <param name="connection">The connection to send messages through.</param>
        /// <returns>The built <see cref="IWampOutgoingMessageHandler"/>.</returns>
        IWampOutgoingMessageHandler Build(TRawClient client, IWampConnection<TMessage> connection);
    }
}