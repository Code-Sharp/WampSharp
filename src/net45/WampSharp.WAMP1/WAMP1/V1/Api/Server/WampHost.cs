using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Listener;
using WampSharp.V1.Core.Listener.ClientBuilder;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1
{
    public class WampHost<TMessage> : IWampHost
    {
        private readonly IWampServer<TMessage> mServer;
        private WampListener<TMessage> mListener;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampHost(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter) : 
            this(new WampServerBuilder<TMessage>(), connectionListener, formatter)
        {
        }

        public WampHost(IWampServerBuilder<TMessage> serverBuilder, IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;

            MetadataCatalog = new WampRpcMetadataCatalog();
            
            TopicContainerExtended = new WampTopicContainer<TMessage>();

            mServer = serverBuilder.Build(formatter, MetadataCatalog, TopicContainerExtended);

            mListener = GetWampListener(connectionListener, formatter, mServer);
		}

        private static WampListener<TMessage> GetWampListener(IWampConnectionListener<TMessage> connectionListener, IWampFormatter<TMessage> formatter, IWampServer<TMessage> server)
        {
            IWampClientBuilderFactory<TMessage, IWampClient> clientBuilderFactory =
                GetWampClientBuilder(formatter);
            
            IWampClientContainer<TMessage, IWampClient> clientContainer =
                new WampClientContainer<TMessage>(clientBuilderFactory);
            
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
            WampOutgoingRequestSerializer<TMessage> wampOutgoingRequestSerializer = 
                new WampOutgoingRequestSerializer<TMessage>(formatter);

            WampOutgoingMessageHandlerBuilder<TMessage> wampOutgoingMessageHandlerBuilder = 
                new WampOutgoingMessageHandlerBuilder<TMessage>();

            return new WampClientBuilderFactory<TMessage>
                (wampOutgoingRequestSerializer,
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
            MetadataCatalog.Register(new MethodInfoWampRpcMetadata(instance, baseUri));
        }
        
        public void Register(IWampRpcMetadata rpcMetadata)
        {
            MetadataCatalog.Register(rpcMetadata);
        }
        
        public void Unregister(IWampRpcMethod method)
        {
            MetadataCatalog.Unregister(method);
        }

        public IWampTopicContainer TopicContainer => TopicContainerExtended;

        protected IWampTopicContainerExtended<TMessage> TopicContainerExtended { get; }

        protected IWampRpcMetadataCatalog MetadataCatalog { get; }

        protected IWampFormatter<TMessage> Formatter => mFormatter;

        public event EventHandler<WampSessionEventArgs> SessionCreated
        {
            add => mListener.SessionCreated += value;
            remove => mListener.SessionCreated -= value;
        }

        public event EventHandler<WampSessionEventArgs> SessionClosed
        {
            add => mListener.SessionClosed += value;
            remove => mListener.SessionClosed -= value;
        }
    }
}