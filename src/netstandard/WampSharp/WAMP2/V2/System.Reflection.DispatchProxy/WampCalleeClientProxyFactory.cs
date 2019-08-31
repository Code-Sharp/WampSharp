#if DISPATCH_PROXY

using System;
using System.Reflection;
using WampSharp.V2.Client;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : IWampCalleeProxyFactory
    {
        private readonly IWampRealmProxy mProxy;
        private readonly WampCalleeProxyInvocationHandler mHandler;

        public WampCalleeClientProxyFactory(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mHandler = new ClientInvocationHandler(proxy);
        }

        public virtual TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            if (typeof(CalleeProxyBase).IsAssignableFrom(typeof(TProxy)))
            {
                return (TProxy) Activator.CreateInstance(typeof(TProxy), mProxy, interceptor);
            }
            else
            {
                TProxy result = DispatchProxy.Create<TProxy, CalleeProxy>();

                CalleeProxy casted = result as CalleeProxy;

                casted.Handler = mHandler;
                casted.CalleeProxyInterceptor = interceptor;

                return result;
            }
        }
    }
}

#endif