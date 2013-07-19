using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.PubSub.Client
{
    public class WampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampServerProxyBuilder<TMessage, IWampPubSubClient<TMessage>, IWampServer> mProxyBuilder;

        public WampServerProxyFactory(IWampServerProxyBuilder<TMessage, IWampPubSubClient<TMessage>, IWampServer> proxyBuilder)
        {
            mProxyBuilder = proxyBuilder;
        }

        public IWampServer Create(IWampPubSubClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            return mProxyBuilder.Create(client, connection);
        }
    }
}