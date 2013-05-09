using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.Rpc
{
    public class WampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServer> mProxyBuilder;

        public WampServerProxyFactory(IWampConnection<TMessage> connection,
                                      IWampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServer> proxyBuilder)
        {
            mConnection = connection;
            mProxyBuilder = proxyBuilder;
        }

        public IWampServer Create(IWampClient<TMessage> client)
        {
            return mProxyBuilder.Create(client, mConnection);
        }
    }
}