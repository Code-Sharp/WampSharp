using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    /// <summary>
    /// Represents a WAMP rpc operation.
    /// </summary>
    public interface IWampRpcOperation
    {
        /// <summary>
        /// Gets the procedure uri.
        /// </summary>
        string Procedure { get; }

        /// <summary>
        /// Invokes the procedure.
        /// </summary>
        /// <param name="caller">The callback to be notified when a result or error arrives.</param>
        /// <param name="formatter">The formatter that can be used to deserialize call arguments.</param>
        /// <param name="details">The details of this invocation.</param>
        /// <typeparam name="TMessage"></typeparam>
        IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details);

        /// <summary>
        /// Invokes the procedure.
        /// </summary>
        /// <param name="caller">The callback to be notified when a result or error arrives.</param>
        /// <param name="formatter">The formatter that can be used to deserialize call arguments.</param>
        /// <param name="details">The details of this invocation.</param>
        /// <param name="arguments">The arguments associated with this invocation.</param>
        /// <typeparam name="TMessage"></typeparam>
        IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments);

        /// <summary>
        /// Invokes the procedure.
        /// </summary>
        /// <param name="caller">The callback to be notified when a result or error arrives.</param>
        /// <param name="formatter">The formatter that can be used to deserialize call arguments.</param>
        /// <param name="details">The details of this invocation.</param>
        /// <param name="arguments">The arguments associated with this invocation.</param>
        /// <param name="argumentsKeywords">The arguments keywords associated with this invocation.</param>
        /// <typeparam name="TMessage"></typeparam>
        IWampCancellableInvocation Invoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}