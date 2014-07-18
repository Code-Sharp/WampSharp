using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener.ClientBuilder;

namespace WampSharp.Tests.TestHelpers.Integration
{
    public class WampClientPlayground
    {
        private readonly WampChannelFactory mWampChannelFactory =
            new WampChannelFactory();

        public IWampChannel GetChannel<TMessage>(IWampServer<TMessage> serverMock,
                                         string realm,
                                         IWampBinding<TMessage> binding)
        {
            MockConnection<TMessage> connection = new MockConnection<TMessage>();
            
            IWampConnection<TMessage> serverConnection = connection.SideAToSideB;
            IWampConnection<TMessage> clientConnection = connection.SideBToSideA;

            BuildServerMockHandler(serverMock, binding, serverConnection);

            IWampChannel channel = mWampChannelFactory.CreateChannel
                (realm,
                 (IControlledWampConnection<TMessage>) clientConnection,
                 binding);

            return channel;
        }

        private static void BuildServerMockHandler<TMessage>(IWampServer<TMessage> serverMock, IWampBinding<TMessage> binding,
                                                             IWampConnection<TMessage> serverConnection)
        {
            WampClientBuilderFactory<TMessage> factory =
                new WampClientBuilderFactory<TMessage>
                    (new WampIdGenerator(),
                     new WampOutgoingRequestSerializer<TMessage>(binding.Formatter),
                     new WampOutgoingMessageHandlerBuilder<TMessage>(),
                     binding);

            WampClientContainer<TMessage, IWampClient<TMessage>> container =
                new WampClientContainer<TMessage, IWampClient<TMessage>>(factory);

            WampIncomingMessageHandler<TMessage, IWampClient<TMessage>> incomingMessageHandler =
                new WampIncomingMessageHandler<TMessage, IWampClient<TMessage>>
                    (new WampRequestMapper<TMessage>(serverMock.GetType(), binding.Formatter),
                     new WampMethodBuilder<TMessage, IWampClient<TMessage>>(serverMock, binding.Formatter));

            IWampClient<TMessage> proxy = container.GetClient(serverConnection);

            serverConnection.MessageArrived +=
                (sender, args) => incomingMessageHandler.HandleMessage(proxy, args.Message);
        }
    }
}