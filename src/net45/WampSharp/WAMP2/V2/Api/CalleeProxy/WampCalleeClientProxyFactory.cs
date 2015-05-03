using WampSharp.V2.Client;

#if !PCL
namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : WampCalleeProxyFactory
    {
        public WampCalleeClientProxyFactory(IWampRealmProxy realmProxy) :
            base(new ClientInvocationHandler(realmProxy))
        {
        }
    }
}
#endif