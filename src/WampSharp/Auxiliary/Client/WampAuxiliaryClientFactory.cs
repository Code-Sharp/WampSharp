using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;

namespace WampSharp.Auxiliary.Client
{
    public class WampAuxiliaryClientFactory<TMessage> : IWampAuxiliaryClientFactory<TMessage>
    {
        private readonly IWampServerProxyBuilder<TMessage, IWampAuxiliaryClient, IWampServer> mServerProxyBuilder;

        public WampAuxiliaryClientFactory(IWampServerProxyBuilder<TMessage, IWampAuxiliaryClient, IWampServer> serverProxyBuilder)
        {
            mServerProxyBuilder = serverProxyBuilder;
        }

        public IWampClientConnectionMonitor CreateMonitor(IWampConnection<TMessage> connection)
        {
            return new WampClientConnectionMonitor<TMessage>(mServerProxyBuilder, connection);
        }
    }
}