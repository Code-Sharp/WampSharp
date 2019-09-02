using System.Reflection;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// A base class interceptor for both synchronous and asynchronous
    /// rpc calls.
    /// </summary>
    public abstract class WampRpcClientInterceptor
    {
        /// <summary>
        /// Creates a new instance of <see cref="WampRpcClientHandlerBuilder{TMessage}"/>.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="clientHandler"></param>
        public WampRpcClientInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler)
        {
            Serializer = serializer;
            ClientHandler = clientHandler;
        }

        /// <summary>
        /// The serializer used in order to serialize method calls.
        /// </summary>
        public IWampRpcSerializer Serializer { get; }

        /// <summary>
        /// The <see cref="IWampRpcClientHandler"/> use in order
        /// to handle serialized <see cref="WampRpcCall"/>s.
        /// </summary>
        public IWampRpcClientHandler ClientHandler { get; }

        /// <summary>
        /// Called when a method is invoked.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arguments"></param>
        public abstract object Invoke(MethodInfo method, object[] arguments);
    }
}