using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Dispatch;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Core.Listener.ClientBuilder;
using WampSharp.V2.Realm;
using WampSharp.V2.Session;

namespace WampSharp.V2
{
    public class WampBindingHost<TMessage> : IDisposable, IWampBindingHost
    {
        private WampListener<TMessage> mListener;
        private readonly IWampSessionServer<TMessage> mSession;
        private readonly WampRealmContainer<TMessage> mRealmContainer;

        public WampBindingHost(IWampRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener, IWampBinding<TMessage> binding)
        {
            WampSessionServer<TMessage> session = new WampSessionServer<TMessage>();

            IWampOutgoingRequestSerializer<TMessage> outgoingRequestSerializer =
                new WampOutgoingRequestSerializer<TMessage>(binding.Formatter);

            IWampEventSerializer<TMessage> eventSerializer = GetEventSerializer(outgoingRequestSerializer);

            mRealmContainer = new WampRealmContainer<TMessage>(realmContainer, session, eventSerializer, binding);

            // TODO: implement the constructor interface pattern.
            session.RealmContainer = mRealmContainer;

            mSession = session;

            mListener = GetWampListener(connectionListener, binding, outgoingRequestSerializer);
        }

        private static IWampEventSerializer<TMessage> GetEventSerializer(
            IWampOutgoingRequestSerializer<TMessage> outgoingSerializer)
        {
            WampMessageSerializerBuilder<TMessage> serializerGenerator =
                new WampMessageSerializerBuilder<TMessage>(outgoingSerializer);

            return serializerGenerator.GetSerializer<IWampEventSerializer<TMessage>>();
        }

        private WampListener<TMessage> GetWampListener(IWampConnectionListener<TMessage> connectionListener, IWampBinding<TMessage> binding, IWampOutgoingRequestSerializer<TMessage> outgoingRequestSerializer)
        {
            IWampClientBuilderFactory<TMessage, IWampClient<TMessage>> clientBuilderFactory =
                GetWampClientBuilder(binding, outgoingRequestSerializer);

            IWampClientContainer<TMessage, IWampClient<TMessage>> clientContainer =
                new WampClientContainer<TMessage, IWampClient<TMessage>>(clientBuilderFactory);

            IWampRequestMapper<TMessage> requestMapper =
                new WampRequestMapper<TMessage>(typeof(WampServer<TMessage>),
                                                binding.Formatter);

            WampRealmMethodBuilder<TMessage> methodBuilder =
                new WampRealmMethodBuilder<TMessage>(mSession, binding.Formatter);

            IWampIncomingMessageHandler<TMessage, IWampClient<TMessage>> wampIncomingMessageHandler =
                new WampIncomingMessageHandler<TMessage, IWampClient<TMessage>>
                    (requestMapper,
                     methodBuilder);

            return new WampListener<TMessage>
                (connectionListener,
                 wampIncomingMessageHandler,
                 clientContainer,
                 mSession);
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
    }
}