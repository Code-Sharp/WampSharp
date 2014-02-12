using WampSharp.Core.Listener;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// An interface that allows to consume rpc client capabilities.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampRpcClientFactory<TMessage>
    {
        /// <summary>
        /// Gets a proxy to the given interface on the given connection.
        /// </summary>
        /// <typeparam name="TProxy"></typeparam>
        /// <param name="connection">The given connection.</param>
        /// <returns>The created proxy.</returns>
        TProxy GetClient<TProxy>(IWampConnection<TMessage> connection) where TProxy : class;

        // TODO: Maybe this shouldn't be part of this interface.
        /// <summary>
        /// Gets a dynamic proxy on the given connection.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <returns>The created proxy.</returns>
        dynamic GetDynamicClient(IWampConnection<TMessage> connection);
    }
}