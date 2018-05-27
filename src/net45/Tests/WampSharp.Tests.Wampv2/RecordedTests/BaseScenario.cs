using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal abstract class BaseScenario
    {
        private readonly object mServer;

        public BaseScenario()
        {
            Binding = new MockBinding();

            ClientBuilder = new WampMockClientBuilder<MockRaw>(Binding);

            mServer = CreateServer();

            Handler =
                new WampIncomingMessageHandler<MockRaw, IWampClientProxy<MockRaw>>
                    (new WampRequestMapper<MockRaw>(mServer.GetType(),
                                                    Binding.Formatter),
                     new WampMethodBuilder<MockRaw, IWampClientProxy<MockRaw>>
                         (mServer, Binding.Formatter));
        }

        protected abstract object CreateServer();

        public MockBinding Binding { get; }

        public WampMockClientBuilder<MockRaw> ClientBuilder { get; }

        public WampIncomingMessageHandler<MockRaw, IWampClientProxy<MockRaw>> Handler { get; }
    }
}