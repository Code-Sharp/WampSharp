using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V1.Auxiliary.Client;
using WampSharp.V1.Core.Client;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.PubSub.Client;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.V1
{
    public class WampChannelFactory<TMessage> : IWampChannelFactory<TMessage>
    {
        private readonly IWampRpcClientFactory<TMessage> mRpcClientFactory;
        private readonly IWampPubSubClientFactory<TMessage> mPubSubClientFactory;
        private readonly ManualWampServerProxyBuilder<TMessage, IWampClient<TMessage>> mServerProxyBuilder;
        private readonly IWampAuxiliaryClientFactory<TMessage> mWampAuxiliaryClientFactory;

        public WampChannelFactory(IWampFormatter<TMessage> formatter)
        {
            Formatter = formatter;

            mRpcClientFactory =
                GetRpcClientFactory();

            mPubSubClientFactory =
                GetPubSubClientFactory();

            mServerProxyBuilder =
                GetServerProxyBuilder<IWampClient<TMessage>>();

            mWampAuxiliaryClientFactory =
                GetConnectionMonitorFactory();
        }

        private WampAuxiliaryClientFactory<TMessage> GetConnectionMonitorFactory()
        {
            ManualWampServerProxyBuilder<TMessage, IWampAuxiliaryClient> proxyBuilder = 
                GetServerProxyBuilder<IWampAuxiliaryClient>();
            
            return new WampAuxiliaryClientFactory<TMessage>(proxyBuilder);
        }

        private IWampPubSubClientFactory<TMessage> GetPubSubClientFactory()
        {
            ManualWampServerProxyBuilder<TMessage, IWampPubSubClient<TMessage>> proxyBuilder = 
                GetServerProxyBuilder<IWampPubSubClient<TMessage>>();

            PubSub.Client.WampServerProxyFactory<TMessage> serverProxyFactory =
                new PubSub.Client.WampServerProxyFactory<TMessage>(proxyBuilder);

            WampPubSubClientFactory<TMessage> result =
                new WampPubSubClientFactory<TMessage>(serverProxyFactory,
                                                      Formatter);

            return result;
        }

        private WampRpcClientFactory<TMessage> GetRpcClientFactory()
        {
            WampRpcSerializer rpcSerializer = 
                new WampRpcSerializer(new WampRpcMethodAttributeProcUriMapper());

            ManualWampServerProxyBuilder<TMessage, IWampRpcClient<TMessage>> serverProxyBuilder = 
                GetServerProxyBuilder<IWampRpcClient<TMessage>>();

            Rpc.Client.WampServerProxyFactory<TMessage> serverProxyFactory =
                new Rpc.Client.WampServerProxyFactory<TMessage>(serverProxyBuilder);
            
            WampRpcClientHandlerBuilder<TMessage> clientHandlerBuilder = 
                new WampRpcClientHandlerBuilder<TMessage>(Formatter, serverProxyFactory);
            
            WampRpcClientFactory<TMessage> result = 
                new WampRpcClientFactory<TMessage>
                    (rpcSerializer,
                     clientHandlerBuilder);

            return result;
        }

        private ManualWampServerProxyBuilder<TMessage, TRawClient> GetServerProxyBuilder<TRawClient>()
        {
            WampOutgoingRequestSerializer<TMessage> outgoingRequestSerializer = 
                new WampOutgoingRequestSerializer<TMessage>(Formatter);

            WampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient> incomingHandlerBuilder = 
                new WampServerProxyIncomingMessageHandlerBuilder<TMessage, TRawClient>(Formatter);
            
            WampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient> outgoingHandlerBuilder = 
                new WampServerProxyOutgoingMessageHandlerBuilder<TMessage, TRawClient>(incomingHandlerBuilder);

            return new ManualWampServerProxyBuilder<TMessage, TRawClient>
                (outgoingRequestSerializer, outgoingHandlerBuilder);
        }

        public IWampChannel<TMessage> CreateChannel(IControlledWampConnection<TMessage> connection)
        {
            return new WampChannel<TMessage>(connection,
                                             mRpcClientFactory,
                                             mPubSubClientFactory,
                                             mServerProxyBuilder,
                                             mWampAuxiliaryClientFactory);
        }

        public IWampFormatter<TMessage> Formatter { get; }
    }
}