using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
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
            protected readonly InvocationDetails mEmptyOptions = new InvocationDetails();

            private readonly IWampRpcOperationCatalog mCatalog;

            public Handler(IWampRpcOperationCatalog catalog)
            {
                mCatalog = catalog;
            }

            protected override void Invoke(CallOptions options, IWampRawRpcOperationClientCallback callback, string procedure, object[] arguments)
            {
                // TODO: We don't use options here, this is not OK,
                // TODO: but will be ok since the router services provider
                // TODO: is going to be obsolete soon.
                mCatalog.Invoke(callback,
                                WampObjectFormatter.Value,
                                mEmptyOptions,
                                procedure,
                                arguments);
            }
        }
    }
}