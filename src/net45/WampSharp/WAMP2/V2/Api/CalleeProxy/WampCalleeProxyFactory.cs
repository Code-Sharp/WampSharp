using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

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

        public TProxy GetProxy<TProxy>(CallOptions callOptions) where TProxy : class
        {
            ProxyGenerationOptions options = new ProxyGenerationOptions() {Selector = new WampCalleProxyInterceptorSelector()};

            TProxy proxy =
                mGenerator.CreateInterfaceProxyWithoutTarget<TProxy>
                    (options,
                        new SyncCalleeProxyInterceptor(mHandler, callOptions),
                        new AsyncCalleeProxyInterceptor(mHandler, callOptions));

            return proxy;
        }
    }
}