using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.Rpc.Client.Dynamic;

namespace WampSharp.Rpc.Client
{
    public class WampRpcClientFactory<TMessage> : IWampRpcClientFactory<TMessage>
    {
        private readonly ProxyGenerator mProxyGenerator = new ProxyGenerator();
        private readonly IWampRpcSerializer mSerializer;
        private readonly IWampRpcClientHandlerBuilder<TMessage> mClientHandlerBuilder;

        public WampRpcClientFactory(IWampRpcSerializer serializer, IWampRpcClientHandlerBuilder<TMessage> clientHandlerBuilder)
        {
            mSerializer = serializer;
            mClientHandlerBuilder = clientHandlerBuilder;
        }

        public TProxy GetClient<TProxy>(IWampConnection<TMessage> connection) where TProxy : class
        {
            IWampRpcClientHandler handler = mClientHandlerBuilder.Build(connection);

            WampRpcClientSyncInterceptor syncInterceptor =
                new WampRpcClientSyncInterceptor(mSerializer, handler);

            WampRpcClientAsyncInterceptor asyncInterceptor =
                new WampRpcClientAsyncInterceptor(mSerializer, handler);

            ProxyGenerationOptions generationOptions =
                new ProxyGenerationOptions {Selector = new WampRpcClientInterceptorSelector()};

            TProxy result =
                mProxyGenerator.CreateInterfaceProxyWithoutTarget<TProxy>
                    (generationOptions,
                     syncInterceptor,
                     asyncInterceptor);

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