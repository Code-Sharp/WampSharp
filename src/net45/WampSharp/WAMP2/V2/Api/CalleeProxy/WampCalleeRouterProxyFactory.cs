using WampSharp.V2.Core;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeRouterProxyFactory : WampCalleeProxyFactory
    {
        public WampCalleeRouterProxyFactory(IWampRpcOperationCatalog catalog) :
            base(new Handler(catalog))
        {
        }

        private class Handler : WampCalleeProxyInvocationHandler
        {
            private readonly IWampRpcOperationCatalog mCatalog;

            public Handler(IWampRpcOperationCatalog catalog)
            {
                mCatalog = catalog;
            }

            protected override void Invoke(IWampRawRpcOperationCallback callback, string procedure, object[] arguments)
            {
                mCatalog.Invoke(callback,
                                WampObjectFormatter.Value,
                                mEmptyOptions,
                                procedure,
                                arguments);
            }
        }
    }
}