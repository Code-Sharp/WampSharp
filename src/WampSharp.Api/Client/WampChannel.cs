using System.Reactive.Subjects;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;
using WampSharp.PubSub.Client;
using WampSharp.Rpc;

namespace WampSharp.Api
{
    public class WampChannel<TMessage> : IWampChannel<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;
        private readonly IWampRpcClientFactory<TMessage> mRpcClientFactory;
        private readonly IWampPubSubClientFactory<TMessage> mPubSubClientFactory;
        private readonly WampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServer> mServerProxyBuilder;

        public WampChannel(IWampConnection<TMessage> connection,
                           IWampRpcClientFactory<TMessage> rpcClientFactory,
                           IWampPubSubClientFactory<TMessage> pubSubClientFactory,
                           WampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServer> serverProxyBuilder)
        {
            mConnection = connection;
            mRpcClientFactory = rpcClientFactory;
            mPubSubClientFactory = pubSubClientFactory;
            mServerProxyBuilder = serverProxyBuilder;
        }

        public IWampServer GetServerProxy(IWampClient<TMessage> callbackClient)
        {
            return mServerProxyBuilder.Create(callbackClient, mConnection);
        }

        public TProxy GetRpcProxy<TProxy>() where TProxy : class
        {
            return mRpcClientFactory.GetClient<TProxy>(mConnection);
        }

        public dynamic GetDynamicRpcProxy()
        {
            return mRpcClientFactory.GetDynamicClient(mConnection);
        }

        public ISubject<T> GetSubject<T>(string topicUri)
        {
            return mPubSubClientFactory.GetSubject<T>(topicUri, mConnection);
        }
    }
}