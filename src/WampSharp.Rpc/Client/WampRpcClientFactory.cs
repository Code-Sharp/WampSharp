using Castle.DynamicProxy;

namespace WampSharp.Rpc
{
    public class WampRpcClientFactory : IWampRpcClientFactory
    {
        private readonly ProxyGenerator mProxyGenerator = new ProxyGenerator();
        private readonly IWampRpcSerializer mSerializer;
        private readonly IWampRpcClientHandlerBuilder mClientHandlerBuilder;

        public WampRpcClientFactory(IWampRpcSerializer serializer, IWampRpcClientHandlerBuilder clientHandlerBuilder)
        {
            mSerializer = serializer;
            mClientHandlerBuilder = clientHandlerBuilder;
        }

        public TProxy GetClient<TProxy>() where TProxy : class
        {
            IWampRpcClientHandler handler = mClientHandlerBuilder.Build();

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
    }
}