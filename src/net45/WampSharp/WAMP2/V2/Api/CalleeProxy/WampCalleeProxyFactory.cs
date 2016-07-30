#if CASTLE
using System.Linq;
using System.Reflection;
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

        public virtual TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            ProxyGenerationOptions options = new ProxyGenerationOptions()
            {
                Selector = new WampCalleeProxyInterceptorSelector()
            };

            IInterceptor[] interceptors =
                typeof (TProxy).GetMethods()
                    .Select(method => BuildInterceptor(method, interceptor))
                    .ToArray();

            TProxy proxy =
                mGenerator.CreateInterfaceProxyWithoutTarget<TProxy>(options, interceptors);

            return proxy;
        }

        private IInterceptor BuildInterceptor(MethodInfo method, ICalleeProxyInterceptor interceptor)
        {
            return CalleeProxyInterceptorFactory.BuildInterceptor(method, interceptor, mHandler);
        }
    }
}
#endif