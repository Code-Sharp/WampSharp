using Castle.DynamicProxy;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// A base class interceptor for both synchronous and asynchronous
    /// rpc calls.
    /// </summary>
    public abstract class WampRpcClientInterceptor : IInterceptor
    {
        private readonly IWampRpcClientHandler mClientHandler;

        /// <summary>
        /// Creates a new instance of <see cref="WampRpcClientHandlerBuilder{TMessage}"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="clientHandler"></param>
        public WampRpcClientInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler)
        {
            Serializer = serializer;
            mClientHandler = clientHandler;
        }

        /// <summary>
        /// The serializer used in order to serialize method calls.
        /// </summary>
        public IWampRpcSerializer Serializer { get; }

        /// <summary>
        /// The <see cref="IWampRpcClientHandler"/> use in order
        /// to handle serialized <see cref="WampRpcCall"/>s.
        /// </summary>
        public IWampRpcClientHandler ClientHandler => mClientHandler;

        /// <summary>
        /// Implementation of <see cref="IInterceptor.Intercept"/>.
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void Intercept(IInvocation invocation);
    }
}