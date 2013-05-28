using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Rpc;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockWampRpcServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly Func<IWampRpcClient<TMessage>, IWampServer> mServerFactory;

        public MockWampRpcServerProxyFactory(Func<IWampRpcClient<TMessage>, IWampServer> serverFactory)
        {
            mServerFactory = serverFactory;
        }

        public IWampServer Create(IWampRpcClient<TMessage> client)
        {
            return mServerFactory(client);
        }
    }
}