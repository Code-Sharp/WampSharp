using WampSharp.Core.Listener;

namespace WampSharp.Core.Client
{
    /// <summary>
    /// Builds a WAMP server proxy.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TRawClient"></typeparam>
    /// <typeparam name="TServer"></typeparam>
    public interface IWampServerProxyBuilder<TMessage, TRawClient, TServer>
    {
        /// <summary>
        /// Creates a WAMP server proxy based on the given connection
        /// which its callbacks will be handled by the given client.
        /// </summary>
        /// <param name="client">The given client which will handle server
        /// callbacks.</param>
        /// <param name="connection">The connection the proxy is based on.</param>
        /// <returns>A proxy to the server.</returns>
        TServer Create(TRawClient client, IWampConnection<TMessage> connection);
    }
}