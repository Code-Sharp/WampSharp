using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Listener;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// A default implementation of <see cref="IWampRpcClientFactory{TMessage}"/>
    /// using dynamic proxy.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampRpcClientFactory<TMessage> : IWampRpcClientFactory<TMessage>
    {
        private readonly IWampRpcSerializer mSerializer;
        private readonly IWampRpcClientHandlerBuilder<TMessage> mClientHandlerBuilder;

        /// <summary>
        /// Creates a new instance of <see cref="WampRpcClientFactory{TMessage}"/>.
        /// </summary>
        /// <param name="serializer">The <see cref="IWampRpcSerializer"/> used
        /// in order to serialize RPC calls.</param>
        /// <param name="clientHandlerBuilder">The <see cref="IWampRpcClientHandlerBuilder{TMessage}"/>
        /// used in order to build the handler that handles the <see cref="WampRpcCall"/>s.</param>
        public WampRpcClientFactory(IWampRpcSerializer serializer, IWampRpcClientHandlerBuilder<TMessage> clientHandlerBuilder)
        {
            mSerializer = serializer;
            mClientHandlerBuilder = clientHandlerBuilder;
        }

        public WampRpcClientInterceptor SelectInterceptor(MethodInfo method,
                                                          IWampRpcClientHandler handler)
        {
            if (typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                return new WampRpcClientSyncInterceptor(mSerializer, handler);
            }

            return new WampRpcClientSyncInterceptor(mSerializer, handler);
        }


        public TProxy GetClient<TProxy>(IWampConnection<TMessage> connection) where TProxy : class
        {
            IWampRpcClientHandler handler = mClientHandlerBuilder.Build(connection);

            WampRpcClientSyncInterceptor syncInterceptor =
                new WampRpcClientSyncInterceptor(mSerializer, handler);

            WampRpcClientAsyncInterceptor asyncInterceptor =
                new WampRpcClientAsyncInterceptor(mSerializer, handler);

            TProxy result = DispatchProxy.Create<TProxy, RpcDispatchProxy>();

            RpcDispatchProxy dispatchProxy = result as RpcDispatchProxy;

            dispatchProxy.SyncInterceptor = syncInterceptor;
            dispatchProxy.AsyncInterceptor = asyncInterceptor;

            return result;
        }

        public dynamic GetDynamicClient(IWampConnection<TMessage> connection)
        {
            IWampRpcClientHandler handler = mClientHandlerBuilder.Build(connection);

            DynamicWampRpcClient client = new DynamicWampRpcClient(handler, mSerializer);

            return client;
        }
    }
}