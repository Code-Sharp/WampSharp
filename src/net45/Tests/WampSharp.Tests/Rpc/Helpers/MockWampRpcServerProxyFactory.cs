using System;
using WampSharp.Core.Listener;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Rpc.Client;

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