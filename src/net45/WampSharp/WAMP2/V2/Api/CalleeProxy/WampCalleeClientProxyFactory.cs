using System;
using System.Reflection;
using WampSharp.V2.Client;

#if CASTLE
namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : WampCalleeProxyFactory
    {
        private readonly IWampRealmProxy mRealmProxy;

        public WampCalleeClientProxyFactory(IWampRealmProxy realmProxy) :
            base(new ClientInvocationHandler(realmProxy))
        {
            mRealmProxy = realmProxy;
        }

        public override TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor)
        {
            if (typeof(CalleeProxyBase).IsAssignableFrom(typeof(TProxy)))
            {
                return (TProxy)Activator.CreateInstance(typeof(TProxy), mRealmProxy, interceptor);
            }
            else
            {
                return base.GetProxy<TProxy>(interceptor);                
            }
        }
    }
}
#endif