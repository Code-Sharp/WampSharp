using NUnit.Framework;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Tests.Proxy.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts.V1;
using WampSharp.V1.Core.Listener.ClientBuilder.V1;

namespace WampSharp.Tests.Proxy
{
    [TestFixture]
    public class WampClientBuilderTests
    {
        private WampClientBuilder<MockRaw> mBuilder;
        private MockOutgoingMessageHandler mOutgoingMessageHandler;
        private WampMessageEqualityComparer<MockRaw> mComparer;

        [SetUp]
        public void Setup()
        {
            MockRawFormatter formatter = new MockRawFormatter();

            mOutgoingMessageHandler = new MockOutgoingMessageHandler();
            
            mBuilder =
                new WampClientBuilder<MockRaw>(new MockSessionGuidGenerator(),
                    new WampOutgoingRequestSerializer<MockRaw>(formatter),
                    new MockOutgoingMessageHandlerBuilder(mOutgoingMessageHandler),
                    new MockClientContainer());

            mComparer =
                new WampMessageEqualityComparer<MockRaw>
                    (new MockRawComparer());
        }

        [Test]
        public void Welcome()
        {
            MockConnection<MockRaw> connection = new MockConnection<MockRaw>();
            IWampClient client = mBuilder.Create(connection.SideAToSideB);

            client.Welcome("v59mbCGDXZ7WTyxB", 1, "Autobahn/0.5.1");

            WampMessage<MockRaw> serialized =
                mOutgoingMessageHandler.Message;

            WampMessage<MockRaw> raw = 
                WampV1Messages.Welcome("v59mbCGDXZ7WTyxB", 1, "Autobahn/0.5.1");

            Assert.That(serialized, Is.EqualTo(raw).Using(mComparer));
        }
    }
}