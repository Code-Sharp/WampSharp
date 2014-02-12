using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V1.Auxiliary.Server;
using WampSharp.V1.Core.Contracts.V1;
using WampSharp.V1.Core.Listener;
using WampSharp.V1.Core.Listener.ClientBuilder.V1;
using WampSharp.V1.Core.Listener.V1;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1
{
    public class WampHost<TMessage> : IWampHost
    {
        private readonly IWampServer<TMessage> mServer;
        private WampListener<TMessage> mListener;
        private readonly WampRpcMetadataCatalog mMetadataCatalog;
        private readonly IWampTopicContainerExtended<TMessage> mTopicContainer;

        public WampHost(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter)
        {
            mMetadataCatalog = new WampRpcMetadataCatalog();
            WampRpcServer<TMessage> rpcServer = new WampRpcServer<TMessage>(formatter, mMetadataCatalog);
            
            mTopicContainer = new WampTopicContainer<TMessage>();
            WampPubSubServer<TMessage> pubSubServer = new WampPubSubServer<TMessage>(mTopicContainer);

            WampAuxiliaryServer auxiliaryServer = new WampAuxiliaryServer();

            mServer = new DefaultWampServer<TMessage>(rpcServer, pubSubServer, auxiliaryServer);

            mListener = GetWampListener(connectionListener, formatter, mServer);
        }

        private static WampListener<TMessage> GetWampListener(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter, IWampServer<TMessage> server)
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

            return new WampListener<TMessage>
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

        public void HostService(object instance, string baseUri)
        {
            mMetadataCatalog.Register(new MethodInfoWampRpcMetadata(instance, baseUri));
        }

        public IWampTopicContainer TopicContainer
        {
            get
            {
                return mTopicContainer;
            }
        }
    }
}