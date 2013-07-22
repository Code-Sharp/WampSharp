using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;
using WampSharp.Rpc;
using WampSharp.Rpc.Client;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockWampRpcServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly Func<IWampRpcClient<TMessage>, IWampServer> mServerFactory;

        public MockWampRpcServerProxyFactory(Func<IWampRpcClient<TMessage>, IWampServer> serverFactory)
        {
            mServerFactory = serverFactory;
        }

        public IWampServer Create(IWampRpcClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            return mServerFactory(client);
        }
    }
}