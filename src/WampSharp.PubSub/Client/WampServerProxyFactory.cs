using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.PubSub.Client
{
    public class WampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampServerProxyBuilder<TMessage, IWampPubSubClient<TMessage>, IWampServer> mProxyBuilder;

        public WampServerProxyFactory(IWampConnection<TMessage> connection,
                                      IWampServerProxyBuilder<TMessage, IWampPubSubClient<TMessage>, IWampServer> proxyBuilder)
        {
            mConnection = connection;
            mProxyBuilder = proxyBuilder;
        }

        public IWampServer Create(IWampPubSubClient<TMessage> client)
        {
            return mProxyBuilder.Create(client, mConnection);
        }
    }
}