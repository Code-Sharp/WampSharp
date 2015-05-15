#if DNX

using System.Reflection;
using WampSharp.V2.Client;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : IWampCalleeProxyFactory
    {
        private readonly WampCalleeProxyInvocationHandler mHandler;

        public WampCalleeClientProxyFactory(IWampRealmProxy proxy)
        {
            mHandler = new ClientInvocationHandler(proxy);
        }

        public virtual TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            TProxy result = DispatchProxy.Create<TProxy, CalleeProxy>();

            CalleeProxy casted = result as CalleeProxy;

            casted.Handler = mHandler;
            casted.CalleeProxyInterceptor = interceptor;

            return result;
        }
    }
}

#endif