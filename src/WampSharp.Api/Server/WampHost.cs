using System;
using Newtonsoft.Json.Linq;
using WampSharp.Auxiliary;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Listener.V1;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp.Api
{
    public class WampHost : WampHost<JToken>
    {
        public WampHost(string location) : 
            base(location, new JTokenMessageParser(), new JsonFormatter())
        {
        }
    }

    public class WampHost<TMessage> : IWampHost
    {
        private readonly IWampServer<TMessage> mServer;
        private WampListener<TMessage> mListener;
        private readonly WampRpcMetadataCatalog mMetadataCatalog;
        private readonly IWampTopicContainerExtended<TMessage> mTopicContainer;

        public WampHost(string location,
                        IWampMessageParser<TMessage> parser,
                        IWampFormatter<TMessage> formatter) :
                            this(new FleckWampConnectionListener<TMessage>(location, parser), formatter)
        {
        }

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