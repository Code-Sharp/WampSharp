using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Handles messages of a WAMP2 dealer.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <remarks>These messages are part of the WAMP2 specification.</remarks>
    public interface IWampDealer<TMessage> : IWampRpcInvocationCallback<TMessage>,
                                             IWampErrorCallback<TMessage>
    {
        /// <summary>
        /// Occurs when a REGISTER message arrives.
        /// </summary>
        /// <param name="callee">The callee that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="procedure">The uri of the procedure to register.</param>
        [WampHandler(WampMessageType.v2Register)]
        void Register([WampProxyParameter] IWampCallee callee, long requestId, RegisterOptions options, string procedure);

        /// <summary>
        /// Occurs when an UNREGISTER message arrives.
        /// </summary>
        /// <param name="callee">The callee that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="registrationId">The registration id of the registration to remove.</param>
        [WampHandler(WampMessageType.v2Unregister)]
        void Unregister([WampProxyParameter]IWampCallee callee, long requestId, long registrationId);

        /// <summary>
        /// Occurs when a CALL message arrives.
        /// </summary>
        /// <param name="caller">The caller that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The call request options.</param>
        /// <param name="procedure">The uri of the procedure to call.</param>
        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampCaller caller, long requestId, CallOptions options, string procedure);

        /// <summary>
        /// Occurs when a CALL message arrives.
        /// </summary>
        /// <param name="caller">The caller that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The call request options.</param>
        /// <param name="procedure">The uri of the procedure to call.</param>
        /// <param name="arguments">The arguments of the procedure to call.</param>
        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments);

        /// <summary>
        /// Occurs when a CALL message arrives.
        /// </summary>
        /// <param name="caller">The caller that sent this message.</param>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The call request options.</param>
        /// <param name="procedure">The uri of the procedure to call.</param>
        /// <param name="arguments">The arguments of the procedure to call.</param>
        /// <param name="argumentsKeywords">The argument keywords of the procedure to call.</param>
        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        /// <summary>
        /// Occurs when a CANCEL message arrives.
        /// </summary>
        /// <param name="caller">The caller that sent this message.</param>
        /// <param name="requestId">The request id of the call to cancel.</param>
        /// <param name="options">Additional options for cancelation.</param>
        [WampHandler(WampMessageType.v2Cancel)]
        void Cancel([WampProxyParameter] IWampCaller caller, long requestId, CancelOptions options);
    }
}