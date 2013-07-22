using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.Rpc.Client
{
    public class WampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampServerProxyBuilder<TMessage, IWampRpcClient<TMessage>, IWampServer> mProxyBuilder;

        public WampServerProxyFactory(IWampServerProxyBuilder<TMessage, IWampRpcClient<TMessage>, IWampServer> proxyBuilder)
        {
            mProxyBuilder = proxyBuilder;
        }

        public IWampServer Create(IWampRpcClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            return mProxyBuilder.Create(client, connection);
        }
    }
}