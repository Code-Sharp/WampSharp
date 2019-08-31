using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// An implementation of <see cref="IWampServerProxyFactory{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampServerProxyFactory<TMessage> : IWampServerProxyFactory<TMessage>
    {
        private readonly IWampServerProxyBuilder<TMessage, IWampRpcClient<TMessage>, IWampServer> mProxyBuilder;

        /// <summary>
        /// Creates a new instance of <see cref="WampServerProxyFactory{TMessage}"/>.
        /// </summary>
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