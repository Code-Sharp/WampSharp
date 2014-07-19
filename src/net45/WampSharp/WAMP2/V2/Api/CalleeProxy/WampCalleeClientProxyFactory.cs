using WampSharp.V2.Client;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeClientProxyFactory : WampCalleeProxyFactory
    {
        public WampCalleeClientProxyFactory(IWampRpcOperationCatalogProxy catalogProxy) : 
            base(new Handler(catalogProxy))
        {
        }

        private class Handler : WampCalleeProxyInvocationHandler
        {
            private readonly IWampRpcOperationCatalogProxy mCatalogProxy;

            public Handler(IWampRpcOperationCatalogProxy catalogProxy)
            {
                mCatalogProxy = catalogProxy;
            }

            protected override void Invoke(IWampRawRpcOperationCallback callback, string procedure, object[] arguments)
            {
                mCatalogProxy.Invoke(callback,
                                     mEmptyOptions,
                                     procedure,
                                     arguments);
            }
        }
    }
}