using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeProxyFactory : IWampCalleeProxyFactory
    {
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly WampCalleeProxyInvocationHandler mHandler;

        public WampCalleeProxyFactory(WampCalleeProxyInvocationHandler handler)
        {
            mHandler = handler;
        }

        public TProxy GetProxy<TProxy>() where TProxy : class
        {
            ProxyGenerationOptions options = new ProxyGenerationOptions() {Selector = new WampCalleProxyInterceptorSelector()};

            TProxy proxy =
                mGenerator.CreateInterfaceProxyWithoutTarget<TProxy>
                    (options,
                     new SyncCalleeProxyInterceptor(mHandler),
                     new AsyncCalleeProxyInterceptor(mHandler));

            return proxy;
        }
    }
}