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
        private MockBinding mBinding;
        private WampMockClientBuilder<MockRaw> mClientBuilder;
        private object mServer;
        private WampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> mHandler;

        public BaseScenario()
        {
            mBinding = new MockBinding();

            mClientBuilder = new WampMockClientBuilder<MockRaw>(Binding.Formatter);

            mServer = CreateServer();

            mHandler =
                new WampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>>
                    (new WampRequestMapper<MockRaw>(mServer.GetType(),
                                                    Binding.Formatter),
                     new WampMethodBuilder<MockRaw, IWampClient<MockRaw>>
                         (mServer, Binding.Formatter));
        }

        protected abstract object CreateServer();

        public MockBinding Binding
        {
            get { return mBinding; }
        }

        public WampMockClientBuilder<MockRaw> ClientBuilder
        {
            get { return mClientBuilder; }
        }

        public WampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> Handler
        {
            get { return mHandler; }
        }
    }
}