using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Core.Listener.ClientBuilder;

namespace WampSharp.Tests.TestHelpers.Integration
{
    public class WampClientPlayground
    {
        private readonly WampChannelFactory mWampChannelFactory =
            new WampChannelFactory();

        public IWampChannel GetChannel<TMessage>
            (IWampServer<TMessage> serverMock,
                string realm,
                IWampBinding<TMessage> binding)
        {
            return GetChannel(serverMock, realm, binding, new DefaultWampClientAuthenticator());
        }

        public IWampChannel GetChannel<TMessage>(IWampServer<TMessage> serverMock,
                                                 string realm,
                                                 IWampBinding<TMessage> binding,
                                                 IWampClientAuthenticator authenticator)
        {
            MockConnection<TMessage> connection = new MockConnection<TMessage>(binding.Formatter);

            IWampConnection<TMessage> serverConnection = connection.SideAToSideB;
            IWampConnection<TMessage> clientConnection = connection.SideBToSideA;

            BuildServerMockHandler(serverMock, binding, serverConnection);

            IWampChannel channel =
                mWampChannelFactory.CreateChannel
                    (realm,
                        (IControlledWampConnection<TMessage>) clientConnection,
                        binding,
                        authenticator);

            return channel;
        }

        private static void BuildServerMockHandler<TMessage>(IWampServer<TMessage> serverMock, IWampBinding<TMessage> binding,
                                                             IWampConnection<TMessage> serverConnection)
        {
            WampClientBuilderFactory<TMessage> factory =
                new WampClientBuilderFactory<TMessage>
                    (new WampOutgoingRequestSerializer<TMessage>(binding.Formatter),
                     new WampOutgoingMessageHandlerBuilder<TMessage>(),
                     binding);

            WampClientContainer<TMessage> container =
                new WampClientContainer<TMessage>(factory);

            WampIncomingMessageHandler<TMessage, IWampClientProxy<TMessage>> incomingMessageHandler =
                new WampIncomingMessageHandler<TMessage, IWampClientProxy<TMessage>>
                    (new WampRequestMapper<TMessage>(serverMock.GetType(), binding.Formatter),
                     new WampMethodBuilder<TMessage, IWampClientProxy<TMessage>>(serverMock, binding.Formatter));

            IWampClientProxy<TMessage> proxy = container.GetClient(serverConnection);

            serverConnection.MessageArrived +=
                (sender, args) => incomingMessageHandler.HandleMessage(proxy, args.Message);
        }
    }
}