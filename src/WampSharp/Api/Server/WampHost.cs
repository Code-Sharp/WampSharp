using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Listener.V1;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.PubSub.Server;
using WampSharp.Rpc.Server;

namespace WampSharp
{
    public class WampHost<TMessage> : IWampHost
    {
        private readonly IWampServer<TMessage> mServer;
        private WampListener<TMessage> mListener;
        private readonly IWampRpcMetadataCatalog mMetadataCatalog;
        private readonly IWampTopicContainerExtended<TMessage> mTopicContainer;

        public WampHost(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter) : 
            this(new WampServerBuilder<TMessage>(), connectionListener, formatter)
        {
        }

        public WampHost(IWampServerBuilder<TMessage> serverBuilder, IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter)
        {
            mMetadataCatalog = new WampRpcMetadataCatalog();
            
            mTopicContainer = new WampTopicContainer<TMessage>();

            mServer = serverBuilder.Build(formatter, mMetadataCatalog, mTopicContainer);

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
        
        public void Register(IWampRpcMetadata rpcMetadata)
        {
            mMetadataCatalog.Register(rpcMetadata);
        }
        
        public void Unregister(IWampRpcMethod method)
        {
            mMetadataCatalog.Unregister(method);
        }

        public IWampTopicContainer TopicContainer
        {
            get
            {
                return mTopicContainer;
            }
        }

        public event EventHandler<WampSessionEventArgs> SessionCreated
        {
            add
            {
                mListener.SessionCreated += value;
            }
            remove
            {
                mListener.SessionCreated -= value;
            }
        }

        public event EventHandler<WampSessionEventArgs> SessionClosed
        {
            add
            {
                mListener.SessionClosed += value;
            }
            remove
            {
                mListener.SessionClosed -= value;
            }
        }
    }
}