using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.PubSub.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyFactory{TMessage}"/> using
    /// <see cref="IWampServerProxyBuilder{TMessage,TRawClient,TServer}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampServerProxyBuilder<TMessage, IWampPubSubClient<TMessage>, IWampServer> mProxyBuilder;

        /// <summary>
        /// Initializes a new instance of <see cref="WampServerProxyFactory{TMessage}"/>.
        /// </summary>
        /// <param name="proxyBuilder">The <see cref="IWampServerProxyBuilder{TMessage,TRawClient,TServer}"/>
        /// used in order to create the server proxy.</param>
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