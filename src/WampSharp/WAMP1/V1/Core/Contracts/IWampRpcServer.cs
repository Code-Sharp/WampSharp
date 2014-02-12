using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// Contains the RPC methods of a WAMP server.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampRpcServer<TMessage>
    {
        /// <summary>
        /// Occurs when a client calls a rpc method.
        /// </summary>
        /// <param name="client">The client who sent the message.</param>
        /// <param name="callId">An id of this call.</param>
        /// <param name="procUri">An uri representing the method to call.</param>
        /// <param name="arguments">The arguments of the method to call.</param>
        [WampHandler(WampMessageType.v1Call)]
        void Call([WampProxyParameter] IWampClient client, string callId, string procUri, params TMessage[] arguments);
    }
}