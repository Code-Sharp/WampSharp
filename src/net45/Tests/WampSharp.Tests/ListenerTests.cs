using System;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Listener;

namespace WampSharp.Tests
{
    [TestFixture]
    public class ListenerTests
    {
        private IWampFormatter<JToken> mFormatter;

        [SetUp]
        public void Setup()
        {
            mFormatter = new JsonFormatter();
        }

        [Test]
        public void Welcome_OnClient_Connect()
        {
            var clientMock = new Mock<IWampClient>();
            string sessionId = "dfgewj324908";
            clientMock.SetupGet(x => x.SessionId).Returns(sessionId);
            MockListener<JToken> mockListener = new MockListener<JToken>();
            WampListener<JToken> listener = GetListener(mockListener, clientMock.Object);
            listener.Start();
            Mock<IWampConnection<JToken>> connectionMock = new Mock<IWampConnection<JToken>>();
            
            mockListener.OnNext(connectionMock.Object);
            
            connectionMock.Raise(x => x.ConnectionOpen += (sender, args) => {},
                                 EventArgs.Empty);
            
            clientMock.Verify(x => x.Welcome(sessionId, 1, "WampSharp"));
        }

        private WampListener<JToken> GetListener(IWampConnectionListener<JToken> listener, IWampClient wampClient)
        {
            Mock<IWampServer<JToken>> mock = new Mock<IWampServer<JToken>>();

            Mock<IWampClientBuilder<JToken, IWampClient>> clientBuilderMock =
                new Mock<IWampClientBuilder<JToken, IWampClient>>();

            clientBuilderMock.Setup
                (x => x.Create(It.IsAny<IWampConnection<JToken>>()))
                             .Returns(wampClient);

            IWampIncomingMessageHandler<JToken, IWampClient> handler = GetHandler(mock.Object);

            Mock<IWampClientBuilderFactory<JToken, IWampClient>> factory =
                new Mock<IWampClientBuilderFactory<JToken, IWampClient>>();

            factory.Setup(x => x.GetClientBuilder(It.IsAny<IWampClientContainer<JToken, IWampClient>>()))
                .Returns(clientBuilderMock.Object);

            return new WampListener<JToken>
                (listener,
                 handler,
                 new WampClientContainer<JToken>(factory.Object));
        }

        private IWampIncomingMessageHandler<JToken, IWampClient> GetHandler(IWampServer<JToken> wampServer)
        {
            IWampIncomingMessageHandler<JToken, IWampClient> handler =
                new WampIncomingMessageHandler<JToken, IWampClient>
                    (new WampRequestMapper<JToken>(wampServer.GetType(),
                                                   mFormatter),
                     new WampMethodBuilder<JToken, IWampClient>(wampServer, mFormatter));

            return handler;
        }

    }
}
