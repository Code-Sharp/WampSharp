using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Represents a proxy to a WAMP2 dealer.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <remarks>These messages are part of the WAMP2 specification.</remarks>
    public interface IWampDealerProxy<TMessage>
    {
        /// <summary>
        /// Sends a REGISTER message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The request options.</param>
        /// <param name="procedure">The uri of the procedure to register.</param>
        [WampHandler(WampMessageType.v2Register)]
        void Register(long requestId, RegisterOptions options, string procedure);

        /// <summary>
        /// Sends an UNREGISTER message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="registrationId">The registration id of the registration to remove.</param>
        [WampHandler(WampMessageType.v2Unregister)]
        void Unregister(long requestId, long registrationId);

        /// <summary>
        /// Sends a CALL message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The call request options.</param>
        /// <param name="procedure">The uri of the procedure to call.</param>
        [WampHandler(WampMessageType.v2Call)]
        void Call(long requestId, CallOptions options, string procedure);

        /// <summary>
        /// Sends a CALL message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The call request options.</param>
        /// <param name="procedure">The uri of the procedure to call.</param>
        /// <param name="arguments">The arguments of the procedure to call.</param>
        [WampHandler(WampMessageType.v2Call)]
        void Call(long requestId, CallOptions options, string procedure, TMessage[] arguments);

        /// <summary>
        /// Sends a CALL message.
        /// </summary>
        /// <param name="requestId">The request id of the message.</param>
        /// <param name="options">The call request options.</param>
        /// <param name="procedure">The uri of the procedure to call.</param>
        /// <param name="arguments">The arguments of the procedure to call.</param>
        /// <param name="argumentsKeywords">The argument keywords of the procedure to call.</param>
        [WampHandler(WampMessageType.v2Call)]
        void Call(long requestId, CallOptions options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        /// <summary>
        /// Sends a CANCEL message.
        /// </summary>
        /// <param name="requestId">The request id of the call to cancel.</param>
        /// <param name="options">Additional options for cancelation.</param>
        [WampHandler(WampMessageType.v2Cancel)]
        void Cancel(long requestId, CancelOptions options);

        /// <summary>
        /// Sends a YIELD message.
        /// </summary>
        /// <param name="requestId">The request id (given in 
        ///     <see cref="IWampCallee{TMessage}.Invocation(long,long,InvocationDetails)"/> message).</param>
        /// <param name="options">Additional options.</param>
        [WampHandler(WampMessageType.v2Yield)]
        void Yield(long requestId, YieldOptions options);

        /// <summary>
        /// Sends a YIELD message.
        /// </summary>
        /// <param name="requestId">The request id (given in 
        ///     <see cref="IWampCallee{TMessage}.Invocation(long,long,InvocationDetails)"/> message).</param>
        /// <param name="options">Additional options.</param>
        /// <param name="arguments">The arguments of the current result.</param>
        [WampHandler(WampMessageType.v2Yield)]
        void Yield(long requestId, YieldOptions options, TMessage[] arguments);

        /// <summary>
        /// Sends a YIELD message.
        /// </summary>
        /// <param name="requestId">The request id (given in 
        ///     <see cref="IWampCallee{TMessage}.Invocation(long,long,InvocationDetails)"/> message).</param>
        /// <param name="options">Additional options.</param>
        /// <param name="arguments">The arguments of the current result.</param>
        /// <param name="argumentsKeywords">The argument keywords of the current result.</param>
        [WampHandler(WampMessageType.v2Yield)]
        void Yield(long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}