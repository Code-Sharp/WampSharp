using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V1;
using WampSharp.V1.Auxiliary.Server;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Listener;
using WampSharp.V1.Core.Listener.ClientBuilder;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Adapters
{
    public class Wamp1BindingHost<TMessage> : IWampBindingHost
    {
        private V1.Core.Listener.WampListener<TMessage> mListener;
        private readonly IWampRealm mRealm;
        private readonly DefaultWampServer<TMessage> mServer;

        public Wamp1BindingHost(IWampRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter)
        {
            mRealm = realmContainer.GetRealmByName("WAMP1");

            WampRpcServerAdapter<TMessage> rpcServer = new WampRpcServerAdapter<TMessage>(mRealm.RpcCatalog, formatter);

            IWampPubSubServer<TMessage> pubSubServer = new WampPubSubServerAdapter<TMessage>(mRealm.TopicContainer);

            WampAuxiliaryServer auxiliaryServer = new WampAuxiliaryServer();

            mServer = new DefaultWampServer<TMessage>(rpcServer, pubSubServer, auxiliaryServer);

            mListener = GetWampListener(connectionListener, formatter, mServer);
        }

        private static V1.Core.Listener.WampListener<TMessage> GetWampListener(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter, IWampServer<TMessage> server)
        {
            IWampClientBuilderFactory<TMessage, IWampClient> clientBuilderFactory =
                GetWampClientBuilder(formatter);

            IWampClientContainer<TMessage, IWampClient> clientContainer =
                new WampClientContainer<TMessage, IWampClient>(clientBuilderFactory);

            IWampRequestMapper<TMessage> requestMapper =
                new WampRequestMapper<TMessage>(server.GetType(),
                                                formatter);

            WampMethodBuilder<TMessage, IWampClient> methodBuilder =
                new WampMethodBuilder<TMessage, IWampClient>(server, formatter);

            IWampIncomingMessageHandler<TMessage, IWampClient> wampIncomingMessageHandler =
                new WampIncomingMessageHandler<TMessage, IWampClient>
                    (requestMapper,
                     methodBuilder);

            return new V1.Core.Listener.WampListener<TMessage>
                (connectionListener,
                 wampIncomingMessageHandler,
                 clientContainer);
        }

        private static WampClientBuilderFactory<TMessage> GetWampClientBuilder(IWampFormatter<TMessage> formatter)
        {
            WampSessionIdGenerator wampSessionIdGenerator =
                new WampSessionIdGenerator();

            WampOutgoingRequestSerializer<TMessage> wampOutgoingRequestSerializer =
                new WampOutgoingRequestSerializer<TMessage>(formatter);

            WampOutgoingMessageHandlerBuilder<TMessage> wampOutgoingMessageHandlerBuilder =
                new WampOutgoingMessageHandlerBuilder<TMessage>();

            return new WampClientBuilderFactory<TMessage>
                (wampSessionIdGenerator,
                 wampOutgoingRequestSerializer,
                 wampOutgoingMessageHandlerBuilder);
        }

        public void Dispose()
        {
            if (mListener != null)
            {
                mListener.Stop();
                mListener = null;
            }
        }

        public void Open()
        {
            if (mListener == null)
            {
                throw new ObjectDisposedException("mListener");
            }

            mListener.Start();
        }
    }
}