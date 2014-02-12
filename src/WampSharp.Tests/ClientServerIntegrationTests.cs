using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Client;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;
using WampSharp.Newtonsoft;
using WampSharp.Tests.Rpc.Helpers;
using WampSharp.V1.Core.Contracts.V1;
using WampSharp.V1.Core.Listener;
using WampSharp.V1.Core.Listener.ClientBuilder.V1;
using WampSharp.V1.Core.Listener.V1;
using WampSharp.V1.Rpc;
using WampSharp.V1.Rpc.Client;
using WampSharp.V1.Rpc.Server;

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

            WampRpcClientFactory<JToken> factory =
                new WampRpcClientFactory<JToken>(new WampRpcSerializer(new WampDelegateProcUriMapper(x => x.Name)),
                    new WampRpcClientHandlerBuilder<JToken>(mFormatter,
                        new WampServerProxyFactory<JToken>(new WampServerProxyBuilder<JToken, IWampRpcClient<JToken>, IWampServer>(new WampOutgoingRequestSerializer<JToken>(mFormatter),
                                new WampServerProxyOutgoingMessageHandlerBuilder<JToken, IWampRpcClient<JToken>>(new WampServerProxyIncomingMessageHandlerBuilder<JToken, IWampRpcClient<JToken>>(mFormatter))))));

            listener.Start();

            ICalculator calculator = factory.GetClient<ICalculator>(connection.SideAToSideB);

            mockListener.OnNext(connection.SideBToSideA);

            int four = 4;

            int sixteen = calculator.Square(four);

            Assert.That(sixteen, Is.EqualTo(12));

            serverMock.Verify(x => x.Call(It.IsAny<IWampClient>(),
                                          It.IsAny<string>(),
                                          "Square",
                                          It.Is((JToken[] parameters) => parameters[0].Value<int>() == four)));
        }

        public interface IAddCalculator
        {
            int Add(int x, int y);
        }

        public class AddCalculator
        {
            [WampRpcMethod("http://www.yogev.com/pr/add")]
            public int Add(int x, int y)
            {
                return x + y;
                return x + y;
            }
        }

        [Test]
        public void CallServer_And_Receive_Call_Result_ViaRpcServer()
        {
            MockListener<JToken> mockListener = new MockListener<JToken>();

            var wampRpcServiceHost = new WampRpcMetadataCatalog();
            wampRpcServiceHost.Register(new MethodInfoWampRpcMetadata(new AddCalculator()));

            WampRpcServer<JToken> rpcServer =
                new WampRpcServer<JToken>(mFormatter,
                                          wampRpcServiceHost);

            WampListener<JToken> listener = GetListener(mockListener, rpcServer);

            MockConnection<JToken> connection = new MockConnection<JToken>();

            WampRpcClientFactory<JToken> factory =
                new WampRpcClientFactory<JToken>(new WampRpcSerializer(new WampDelegateProcUriMapper(x => "http://www.yogev.com/pr/" + x.Name)),
                    new WampRpcClientHandlerBuilder<JToken>(mFormatter,
                        new WampServerProxyFactory<JToken>(new WampServerProxyBuilder<JToken, IWampRpcClient<JToken>, IWampServer>(new WampOutgoingRequestSerializer<JToken>(mFormatter),
                                new WampServerProxyOutgoingMessageHandlerBuilder<JToken, IWampRpcClient<JToken>>(new WampServerProxyIncomingMessageHandlerBuilder<JToken, IWampRpcClient<JToken>>(mFormatter))))));

            listener.Start();

            IAddCalculator calculator = factory.GetClient<IAddCalculator>(connection.SideAToSideB);

            mockListener.OnNext(connection.SideBToSideA);

            int sixteen = calculator.Add(10, 6);

            Assert.That(sixteen, Is.EqualTo(16));
        }


        
        private IWampServer GetClient(IWampConnection<JToken> connection, IWampClient<JToken> wampClient)
        {
            var serverProxyBuilder = new WampServerProxyBuilder<JToken, IWampClient<JToken>, IWampServer>
                (new WampOutgoingRequestSerializer<JToken>(mFormatter),
                 new WampServerProxyOutgoingMessageHandlerBuilder<JToken, IWampClient<JToken>>
                     (GetHandlerBuilder()));

            var proxy =
                serverProxyBuilder.Create(wampClient, connection);

            return proxy;
        }

        private WampListener<JToken> GetListener(IWampConnectionListener<JToken> listener, object wampServer)
        {
            IWampIncomingMessageHandler<JToken, IWampClient> handler = GetHandler(wampServer);

            return new WampListener<JToken>
                (listener,
                 handler,
                 new WampClientContainer<JToken, IWampClient>
                     (new WampClientBuilderFactory<JToken>
                          (new WampSessionIdGenerator(),
                           new WampOutgoingRequestSerializer<JToken>(mFormatter),
                           new WampOutgoingMessageHandlerBuilder<JToken>())));
        }

        private IWampServerProxyIncomingMessageHandlerBuilder<JToken,IWampClient<JToken>> GetHandlerBuilder()
        {
            return new WampServerProxyIncomingMessageHandlerBuilder<JToken, IWampClient<JToken>>(mFormatter);
        }

        private IWampIncomingMessageHandler<JToken, IWampClient> GetHandler(object instance)
        {
            IWampIncomingMessageHandler<JToken, IWampClient> handler =
                new WampIncomingMessageHandler<JToken, IWampClient>
                    (new WampRequestMapper<JToken>(instance.GetType(),
                                                   mFormatter),
                     new WampMethodBuilder<JToken, IWampClient>(instance, mFormatter));

            return handler;
        }

    }
}