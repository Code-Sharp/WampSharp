using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    /// <summary>
    /// Represents a callback for a WAMP rpc operation that lives in the router.
    /// </summary>
    public interface IWampRawRpcOperationRouterCallback : IWampRawRpcOperationCallback<YieldOptions>
    {
        public long RequestId { get; }
    }

    /// <summary>
    /// Represents a callback for a WAMP rpc operation that lives outside the router.
    /// </summary>
    public interface IWampRawRpcOperationClientCallback : IWampRawRpcOperationCallback<ResultDetails>
    {
    }

    /// <summary>
    /// Represents a callback for a <see cref="IWampRpcOperation"/>.
    /// </summary>
    /// <typeparam name="TDetailsOptions"></typeparam>
    public interface IWampRawRpcOperationCallback<TDetailsOptions>
    {
        /// <summary>
        /// Occurs when a result has arrived.
        /// </summary>
        /// <param name="formatter">The formatter the can be used to deserialize arguments.</param>
        /// <param name="details">The details associated with this result.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Result<TMessage>(IWampFormatter<TMessage> formatter, TDetailsOptions details);

        /// <summary>
        /// Occurs when a result has arrived.
        /// </summary>
        /// <param name="formatter">The formatter the can be used to deserialize arguments.</param>
        /// <param name="details">The details associated with this result.</param>
        /// <param name="arguments">The arguments associated with this result.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Result<TMessage>(IWampFormatter<TMessage> formatter, TDetailsOptions details, TMessage[] arguments);

        /// <summary>
        /// Occurs when a result has arrived.
        /// </summary>
        /// <param name="formatter">The formatter the can be used to deserialize arguments.</param>
        /// <param name="details">The details associated with this result.</param>
        /// <param name="arguments">The arguments associated with this result.</param>
        /// <param name="argumentsKeywords">The arguments keywords associated with this result.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Result<TMessage>(IWampFormatter<TMessage> formatter, TDetailsOptions details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        /// <summary>
        /// Occurs when an error has occurred.
        /// </summary>
        /// <param name="formatter">The formatter that can be used to deserialize arguments.</param>
        /// <param name="details">The details associated with this error.</param>
        /// <param name="error">The error uri.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error);

        /// <summary>
        /// Occurs when an error has occurred.
        /// </summary>
        /// <param name="formatter">The formatter that can be used to deserialize arguments.</param>
        /// <param name="details">The details associated with this error.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The arguments associated with this error.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments);

        /// <summary>
        /// Occurs when an error has occurred.
        /// </summary>
        /// <param name="formatter">The formatter that can be used to deserialize arguments.</param>
        /// <param name="details">The details associated with this error.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The arguments associated with this error.</param>
        /// <param name="argumentsKeywords">The arguments keywords associated with this error.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}