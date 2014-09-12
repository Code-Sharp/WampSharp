using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents a proxy to <see cref="IWampRpcOperationInvoker"/>.
    /// </summary>
    public interface IWampRpcOperationInvokerProxy : IWampRpcOperationInvokerProxy<object>
    {
    }

    /// <summary>
    /// Represents a proxy to <see cref="IWampRpcOperationInvoker"/>.
    /// </summary>
    public interface IWampRpcOperationInvokerProxy<TMessage>
    {
        /// <summary>
        /// Invokes a operation remotely.
        /// </summary>
        /// <param name="caller">The caller that gets operation result callbacks.</param>
        /// <param name="options">The options to invoke the operation with.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        void Invoke(IWampRpcOperationCallback caller, CallOptions options, string procedure);

        /// <summary>
        /// Invokes a operation remotely.
        /// </summary>
        /// <param name="caller">The caller that gets operation result callbacks.</param>
        /// <param name="options">The options to invoke the operation with.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <param name="arguments">The arguments to invoke the operation with.</param>
        void Invoke(IWampRpcOperationCallback caller, CallOptions options, string procedure, TMessage[] arguments);

        /// <summary>
        /// Invokes a operation remotely.
        /// </summary>
        /// <param name="caller">The caller that gets operation result callbacks.</param>
        /// <param name="options">The options to invoke the operation with.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <param name="arguments">The arguments to invoke the operation with.</param>
        /// <param name="argumentsKeywords">The arguments keywords to invoke the operation with.</param>
        void Invoke(IWampRpcOperationCallback caller, CallOptions options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);

        /// <summary>
        /// Invokes a operation remotely.
        /// </summary>
        /// <param name="caller">The caller that gets operation result callbacks.</param>
        /// <param name="options">The options to invoke the operation with.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        void Invoke(IWampRawRpcOperationCallback caller, CallOptions options, string procedure);

        /// <summary>
        /// Invokes a operation remotely.
        /// </summary>
        /// <param name="caller">The caller that gets operation result callbacks.</param>
        /// <param name="options">The options to invoke the operation with.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <param name="arguments">The arguments to invoke the operation with.</param>
        void Invoke(IWampRawRpcOperationCallback caller, CallOptions options, string procedure, TMessage[] arguments);

        /// <summary>
        /// Invokes a operation remotely.
        /// </summary>
        /// <param name="caller">The caller that gets operation result callbacks.</param>
        /// <param name="options">The options to invoke the operation with.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <param name="arguments">The arguments to invoke the operation with.</param>
        /// <param name="argumentsKeywords">The arguments keywords to invoke the operation with.</param>
        void Invoke(IWampRawRpcOperationCallback caller, CallOptions options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);    
    }
}