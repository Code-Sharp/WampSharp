using System.Threading.Tasks;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// Handles outgoing RPC calls.
    /// </summary>
    public interface IWampRpcClientHandler
    {
        /// <summary>
        /// Handles synchronous rpc calls.
        /// </summary>
        /// <param name="rpcCall">The given rpc call.</param>
        /// <returns>The result of the call.</returns>
        object Handle(WampRpcCall rpcCall);

        /// <summary>
        /// Handles asynchronous rpc calls.
        /// </summary>
        /// <param name="rpcCall">The given rpc call.</param>
        /// <returns>The a task that represents result of the call.</returns>
        Task<object> HandleAsync(WampRpcCall rpcCall);
    }
}