using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Handles WAMP2 dealer YIELD messages.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampRpcInvocationCallback<TMessage>
    {
        /// <summary>
        /// Occurs when a YIELD message arrives.
        /// </summary>
        /// <param name="callee">The <see cref="IWampCallee"/> that sent this message.</param>
        /// <param name="requestId">The request id (given in 
        ///     <see cref="IWampCallee{TMessage}.Invocation(long,long,InvocationDetails)"/> message).</param>
        /// <param name="options">Additional options.</param>
        [WampHandler(WampMessageType.v2Yield)]
        void Yield([WampProxyParameter] IWampCallee callee, long requestId, YieldOptions options);

        /// <summary>
        /// Occurs when a YIELD message arrives.
        /// </summary>
        /// <param name="callee">The <see cref="IWampCallee"/> that sent this message.</param>
        /// <param name="requestId">The request id (given in 
        ///     <see cref="IWampCallee{TMessage}.Invocation(long,long,InvocationDetails)"/> message).</param>
        /// <param name="options">Additional options.</param>
        /// <param name="arguments">The arguments of the current result.</param>
        [WampHandler(WampMessageType.v2Yield)]
        void Yield([WampProxyParameter] IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments);

        /// <summary>
        /// Occurs when a YIELD message arrives.
        /// </summary>
        /// <param name="callee">The <see cref="IWampCallee"/> that sent this message.</param>
        /// <param name="requestId">The request id (given in 
        ///     <see cref="IWampCallee{TMessage}.Invocation(long,long,InvocationDetails)"/> message).</param>
        /// <param name="options">Additional options.</param>
        /// <param name="arguments">The arguments of the current result.</param>
        /// <param name="argumentsKeywords">The argument keywords of the current result.</param>
        [WampHandler(WampMessageType.v2Yield)]
        void Yield([WampProxyParameter] IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}