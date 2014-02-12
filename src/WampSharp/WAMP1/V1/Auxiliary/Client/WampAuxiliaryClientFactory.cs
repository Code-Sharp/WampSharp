using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Auxiliary.Client
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