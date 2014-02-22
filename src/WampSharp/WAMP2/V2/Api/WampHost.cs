using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Core.Listener.ClientBuilder;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;
using WampSharp.V2.Session;

namespace WampSharp.V2
{
    public class WampHost<TMessage>
        where TMessage : class
    {
        private readonly IWampServer<TMessage> mServer; 
        private WampListener<TMessage> mListener;
        private readonly WampRpcOperationCatalog<TMessage> mOperationCatalog;

        public WampHost(IWampConnectionListener<TMessage> connectionListener, IWampBinding<TMessage> binding)
        {
            mOperationCatalog = new WampRpcOperationCatalog<TMessage>();

            IWampRpcServer<TMessage> dealer = 
                new WampRpcServer<TMessage>(mOperationCatalog);

            IWampSessionServer<TMessage> session =
                new WampSessionServer<TMessage>();

            IWampOutgoingRequestSerializer<TMessage> outgoingRequestSerializer =
                new WampOutgoingRequestSerializer<TMessage>(binding.Formatter);

            IWampPubSubServer<TMessage> broker =
                new WampPubSubServer<TMessage>();

            mServer = new WampServer<TMessage>(session, dealer, broker);
            
            mListener = GetWampListener(connectionListener, binding, mServer, outgoingRequestSerializer);
        }

        private static WampListener<TMessage> GetWampListener(IWampConnectionListener<TMessage> connectionListener, IWampBinding<TMessage> binding, IWampServer<TMessage> server, IWampOutgoingRequestSerializer<TMessage> outgoingRequestSerializer)
        {
            IWampClientBuilderFactory<TMessage, IWampClient> clientBuilderFactory =
                GetWampClientBuilder(binding, outgoingRequestSerializer);

            IWampClientContainer<TMessage, IWampClient> clientContainer =
                new WampClientContainer<TMessage, IWampClient>(clientBuilderFactory);

            IWampRequestMapper<TMessage> requestMapper =
                new WampRequestMapper<TMessage>(server.GetType(),
                                                binding.Formatter);

            WampMethodBuilder<TMessage, IWampClient> methodBuilder =
                new WampMethodBuilder<TMessage, IWampClient>(server, binding.Formatter);

            IWampIncomingMessageHandler<TMessage, IWampClient> wampIncomingMessageHandler =
                new WampIncomingMessageHandler<TMessage, IWampClient>
                    (requestMapper,
                     methodBuilder);

            return new WampListener<TMessage>
                (connectionListener,
                 wampIncomingMessageHandler,
                 clientContainer,
                 server);
        }

        private static WampClientBuilderFactory<TMessage> GetWampClientBuilder(IWampBinding<TMessage> binding, IWampOutgoingRequestSerializer<TMessage> outgoingRequestSerializer)
        {
            WampIdGenerator wampIdGenerator =
                new WampIdGenerator();

            WampOutgoingMessageHandlerBuilder<TMessage> wampOutgoingMessageHandlerBuilder =
                new WampOutgoingMessageHandlerBuilder<TMessage>();

            return new WampClientBuilderFactory<TMessage>
                (wampIdGenerator,
                 outgoingRequestSerializer,
                 wampOutgoingMessageHandlerBuilder,
                 binding);
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

        public void HostService(object instance)
        {
            Type type = instance.GetType();

            foreach (Type currentType in type.GetInterfaces())
            {
                IEnumerable<MethodInfo> relevantMethods =
                    currentType.GetMethods()
                               .Where(x => x.IsDefined(typeof (WampProcedureAttribute), true));

                foreach (MethodInfo method in relevantMethods)
                {
                    mOperationCatalog.Register
                        (new MethodInfoRpcOperation<TMessage>
                             (method,
                              instance));
                }
            }
        }
    }
}