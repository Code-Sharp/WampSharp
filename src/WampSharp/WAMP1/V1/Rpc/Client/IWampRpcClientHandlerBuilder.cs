using WampSharp.Core.Listener;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// Builds a <see cref="IWampRpcClientHandler"/> given a connection.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampRpcClientHandlerBuilder<TMessage>
    {
        /// <summary>
        /// Builds a <see cref="IWampRpcClientHandler"/> given a <see cref="IWampConnection{TMessage}"/>.
        /// </summary>
        /// <param name="connection">The given <see cref="IWampConnection{TMessage}"/>.</param>
        /// <returns>The built <see cref="IWampRpcClientHandler"/>.</returns>
        IWampRpcClientHandler Build(IWampConnection<TMessage> connection);
    }
}