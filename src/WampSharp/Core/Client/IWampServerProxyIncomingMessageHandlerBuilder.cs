using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// Builds an <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    public interface IWampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient>
    {
        /// <summary>
        /// Creates an <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/>
        /// that will use <paramref name="client"/> in order to handle incoming callbacks.
        /// </summary>
        /// <param name="client">The client that will be used in order to handle incoming callbacks.</param>
        /// <param name="connection">Not sure why this is here.</param>
        /// <returns>The built <see cref="IWampIncomingMessageHandler{TMessage}"/>.</returns>
        IWampIncomingMessageHandler<TMessage> Build(TRawClient client, IWampConnection<TMessage> connection);
    }
}