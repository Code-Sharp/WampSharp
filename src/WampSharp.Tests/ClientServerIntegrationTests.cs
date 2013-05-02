using Moq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Rpc;

namespace WampSharp.Tests
{
    [TestFixture]
    public class ClientServerIntegrationTests
    {
        private IWampFormatter<JToken> mFormatter;

        [SetUp]
        public void Setup()
        {
            mFormatter = new JsonFormatter();
        }

        [Test]
        public void CallServer_And_Receive_Call_Result()
        {
            MockListener<JToken> mockListener = new MockListener<JToken>();

            Mock<IWampServer<JToken>> serverMock = new Mock<IWampServer<JToken>>();
            serverMock.Setup(x => x.Call(It.IsAny<IWampClient>(),
                                         It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<JToken[]>()))
                      .Callback((IWampClient clientParameter, string callId, string procUrl, JToken[] arguments) =>
                                    {
                                        clientParameter.CallResult(callId, new {stuff = "cool"});
                                    });
            
            WampListener<JToken> listener = GetListener(mockListener, serverMock.Object);

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();
            
            MockConnection<JToken> connection = new MockConnection<JToken>();
            
            IWampServer client = GetClient(connection.SideAToSideB, clientMock.Object);

            listener.Start();
            mockListener.OnNext(connection.SideBToSideA);

            clientMock.Verify(x => x.Welcome(It.IsAny<string>(),
                                             It.IsAny<int>(),
                                             It.IsAny<string>()));

            Mock<IWampClient> userClientMock = new Mock<IWampClient>();

            client.Call(userClientMock.Object, "a123123", "Test",
                        new
                            {
                                name = "Jack"
                            });

            serverMock.Verify(x => x.Call(It.IsAny<IWampClient>(),
                                         "a123123",
                                         "Test",
                                         It.IsAny<JToken[]>()));

            clientMock.Verify(x => x.CallResult("a123123",
                                                It.Is((JToken result) => result.Value<string>("stuff") == "cool")));
        }

        [Test]
        public void CallServer_And_Receive_Call_Result_ViaRpcClient()
        {
            MockListener<JToken> mockListener = new MockListener<JToken>();

            Mock<IWampServer<JToken>> serverMock = new Mock<IWampServer<JToken>>();
            serverMock.Setup(x => x.Call(It.IsAny<IWampClient>(),
                                         It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<JToken[]>()))
                      .Callback((IWampClient clientParameter, string callId, string procUrl, JToken[] arguments) =>
                                    {
                                        clientParameter.CallResult(callId, 12);
                                    });

            WampListener<JToken> listener = GetListener(mockListener, serverMock.Object);

            MockConnection<JToken> connection = new MockConnection<JToken>();

            WampRpcClientFactory factory =
                new WampRpcClientFactory(new WampRpcSerializer(new DelegateProcUriMapper(x => x.Name)),
                    new WampRpcClientHandlerBuilder<JToken>(mFormatter,
                        new WampServerProxyFactory<JToken>(connection.SideAToSideB,
                            new WampServerProxyBuilder<JToken>(new WampOutgoingRequestSerializer<JToken>(mFormatter),
                                new WampServerProxyOutgoingMessageHandlerBuilder<JToken>(new WampServerProxyIncomingMessageHandlerBuilder<JToken>(mFormatter))))));

            listener.Start();

            ICalculator calculator = factory.GetClient<ICalculator>();

            mockListener.OnNext(connection.SideBToSideA);

            int four = 4;

            int sixteen = calculator.Square(four);

            Assert.That(sixteen, Is.EqualTo(12));

            serverMock.Verify(x => x.Call(It.IsAny<IWampClient>(),
                                          It.IsAny<string>(),
                                          "Square",
                                          It.Is((JToken[] parameters) => parameters[0].Value<int>() == four)));
        }


        
        private IWampServer GetClient(IWampConnection<JToken> connection, IWampClient<JToken> wampClient)
        {
            var serverProxyBuilder = new WampServerProxyBuilder<JToken>
                (new WampOutgoingRequestSerializer<JToken>(mFormatter),
                 new WampServerProxyOutgoingMessageHandlerBuilder<JToken>
                     (GetHandlerBuilder()));

            var proxy =
                serverProxyBuilder.Create(wampClient, connection);

            return proxy;
        }

        private WampListener<JToken> GetListener(IWampConnectionListener<JToken> listener, IWampServer<JToken> wampServer)
        {
            IWampIncomingMessageHandler<JToken> handler = GetHandler(wampServer);

            return new WampListener<JToken>
                (listener,
                 handler,
                 new WampClientContainer<JToken>
                     (new WampClientBuilderFactory<JToken>
                          (new WampSessionIdGenerator(),
                           new WampOutgoingRequestSerializer<JToken>(mFormatter),
                           new JsonWampOutgoingHandlerBuilder<JToken>())));
        }

        private IWampServerProxyIncomingMessageHandlerBuilder<JToken> GetHandlerBuilder()
        {
            return new WampServerProxyIncomingMessageHandlerBuilder<JToken>(mFormatter);
        }

        private IWampIncomingMessageHandler<JToken> GetHandler(object instance)
        {
            IWampIncomingMessageHandler<JToken> handler =
                new WampIncomingMessageHandler<JToken>
                    (new WampRequestMapper<JToken>(instance.GetType(),
                                                   mFormatter),
                     new WampMethodBuilder<JToken>(instance, mFormatter));

            return handler;
        }

    }
}