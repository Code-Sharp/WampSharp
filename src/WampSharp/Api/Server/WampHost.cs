using System;
using WampSharp.Auxiliary.Server;
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
        public WampListener<TMessage> Listener
        {
        	get {return mListener;}
        }
        private readonly WampRpcMetadataCatalog mMetadataCatalog;
        private readonly IWampTopicContainerExtended<TMessage> mTopicContainer;
        public event EventHandler<WampSessionEventArgs> SessionCreated;
        public event EventHandler<WampSessionEventArgs> SessionClosed;

        public WampHost(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter)
        {
            mMetadataCatalog = new WampRpcMetadataCatalog();
            WampRpcServer<TMessage> rpcServer = new WampRpcServer<TMessage>(formatter, mMetadataCatalog);
            
            mTopicContainer = new WampTopicContainer<TMessage>();
            WampPubSubServer<TMessage> pubSubServer = new WampPubSubServer<TMessage>(mTopicContainer);

            WampAuxiliaryServer auxiliaryServer = new WampAuxiliaryServer();

            mServer = new DefaultWampServer<TMessage>(rpcServer, pubSubServer, auxiliaryServer);

            mListener = GetWampListener(connectionListener, formatter, mServer);
            mListener.SessionCreated += mListener_SessionCreated;
            mListener.SessionClosed += mListener_SessionClosed;
		}

		void mListener_SessionClosed(object sender, WampSessionEventArgs e)
		{
			var sessionClosed = SessionClosed;
			if (sessionClosed != null)
				sessionClosed(sender, e);
		}

		void mListener_SessionCreated(object sender, WampSessionEventArgs e)
		{
			var sessionCreated = SessionCreated;
			if (sessionCreated != null)
				sessionCreated(sender, e);
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
                mListener.SessionCreated -= mListener_SessionCreated;
                mListener.SessionClosed -= mListener_SessionClosed;
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