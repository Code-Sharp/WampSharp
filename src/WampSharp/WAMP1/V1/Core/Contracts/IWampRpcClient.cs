using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V1.Core.Contracts
{
    /// <summary>
    /// An object version of <see cref="IWampRpcClient{TMessage}"/>
    /// </summary>
    public interface IWampRpcClient : IWampRpcClient<object>
    {
    }

    /// <summary>
    /// Contains the RPC methods of a WAMP client.
    /// </summary>
    public interface IWampRpcClient<TMessage>
    {
        /// <summary>
        /// Occurs when a call result arrives.
        /// </summary>
        /// <param name="callId">The call id.</param>
        /// <param name="result">The call result.</param>
        [WampHandler(WampMessageType.v1CallResult)]
        void CallResult(string callId, TMessage result);

        /// <summary>
        /// Occurs when a call error arrives.
        /// </summary>
        /// <param name="callId">The call id.</param>
        /// <param name="errorUri">An uri to a page describing the error.</param>
        /// <param name="errorDesc">The error description.</param>
        [WampHandler(WampMessageType.v1CallError)]
        void CallError(string callId, string errorUri, string errorDesc);

        /// <summary>
        /// Occurs when a call error arrives.
        /// </summary>
        /// <param name="callId">The call id.</param>
        /// <param name="errorUri">An uri to a page describing the error.</param>
        /// <param name="errorDesc">The error description.</param>
        /// <param name="errorDetails">An object representing error details.</param>
        [WampHandler(WampMessageType.v1CallError)]
        void CallError(string callId, string errorUri, string errorDesc, TMessage errorDetails);
    }
}