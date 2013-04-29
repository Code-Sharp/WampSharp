using Castle.DynamicProxy;

namespace WampSharp.Rpc
{
    public interface IWampRpcClientHandlerBuilder : IWampRpcClientHandlerBuilder<object>
    {
        
    }



    
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
            var interceptor = new WampRpcClientInterceptor(mSerializer, mClientHandlerBuilder.Build());
            TProxy result = mProxyGenerator.CreateInterfaceProxyWithoutTarget<TProxy>(interceptor);
            return result;
        }
    }
}