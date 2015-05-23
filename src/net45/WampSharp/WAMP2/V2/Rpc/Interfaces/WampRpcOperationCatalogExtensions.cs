using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal static class WampRpcOperationCatalogExtensions
    {
        /// <summary>
        /// Invokes the request procedure with the given parameters.
        /// </summary>
        /// <param name="invoker">The given <see cref="IWampRpcOperationInvoker"/>.</param>
        /// <param name="caller">The callback that will be called when a result or error is available.</param>
        /// <param name="formatter">A formatter that can be used to deserialize given arguments.</param>
        /// <param name="details">The details associated with this call.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <typeparam name="TMessage"></typeparam>
        public static void Invoke<TMessage>(this IWampRpcOperationInvoker invoker,
                                            IWampRawRpcOperationClientCallback caller,
                                            IWampFormatter<TMessage> formatter,
                                            InvocationDetails details,
                                            string procedure)
        {
            invoker.Invoke(new WampClientRouterCallbackAdapter(caller, details),
                           formatter,
                           details,
                           procedure);
        }

        /// <summary>
        /// Invokes the request procedure with the given parameters.
        /// </summary>
        /// <param name="invoker">The given <see cref="IWampRpcOperationInvoker"/>.</param>
        /// <param name="caller">The callback that will be called when a result or error is available.</param>
        /// <param name="formatter">A formatter that can be used to deserialize given arguments.</param>
        /// <param name="details">The details associated with this call.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <param name="arguments">The arguments associated with this call.</param>
        /// <typeparam name="TMessage"></typeparam>
        public static void Invoke<TMessage>(this IWampRpcOperationInvoker invoker,
                                            IWampRawRpcOperationClientCallback caller,
                                            IWampFormatter<TMessage> formatter,
                                            InvocationDetails details,
                                            string procedure,
                                            TMessage[] arguments)
        {
            invoker.Invoke(new WampClientRouterCallbackAdapter(caller, details),
                           formatter,
                           details,
                           procedure,
                           arguments);
        }

        /// <summary>
        /// Invokes the request procedure with the given parameters.
        /// </summary>
        /// <param name="invoker">The given <see cref="IWampRpcOperationInvoker"/>.</param>
        /// <param name="caller">The callback that will be called when a result or error is available.</param>
        /// <param name="formatter">A formatter that can be used to deserialize given arguments.</param>
        /// <param name="details">The details associated with this call.</param>
        /// <param name="procedure">The procedure to invoke.</param>
        /// <param name="arguments">The arguments associated with this call.</param>
        /// <param name="argumentsKeywords">The arguments keywords associated with this call.</param>
        /// <typeparam name="TMessage"></typeparam>
        public static void Invoke<TMessage>(this IWampRpcOperationInvoker invoker,
                                            IWampRawRpcOperationClientCallback caller,
                                            IWampFormatter<TMessage> formatter,
                                            InvocationDetails details,
                                            string procedure,
                                            TMessage[] arguments,
                                            IDictionary<string, TMessage> argumentsKeywords)
        {
            invoker.Invoke(new WampClientRouterCallbackAdapter(caller, details),
                           formatter,
                           details,
                           procedure,
                           arguments,
                           argumentsKeywords);
        }
    }
}