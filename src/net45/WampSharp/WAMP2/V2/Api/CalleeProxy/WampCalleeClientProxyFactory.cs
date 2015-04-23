#if !PCL
using WampSharp.V2.Client;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : WampCalleeProxyFactory
    {
        public WampCalleeClientProxyFactory(IWampRpcOperationCatalogProxy catalogProxy, IWampClientConnectionMonitor monitor) : 
            base(new ClientInvocationHandler(catalogProxy, monitor))
        {
        }
    }
}
#endif