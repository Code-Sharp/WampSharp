using System.Collections.Generic;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.Tests.Wampv2.MockBuilder;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class DealerScenario
    {
        private readonly MockBinding mBinding;
        private readonly WampMockClientBuilder<MockRaw> mClientBuilder;
        private readonly WampRpcServer<MockRaw> mServer;
        private readonly WampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> mHandler;

        public DealerScenario()
        {
            mBinding = new MockBinding();

            mClientBuilder = new WampMockClientBuilder<MockRaw>(Binding.Formatter);

            mServer =
                new WampRpcServer<MockRaw>
                    (new WampRpcOperationCatalog(),
                     Binding);

            mHandler =
                new WampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>>
                    (new WampRequestMapper<MockRaw>(Server.GetType(),
                                                    Binding.Formatter),
                     new WampMethodBuilder<MockRaw, IWampClient<MockRaw>>
                         (Server, Binding.Formatter));

        }

        public MockClient<IWampClient<MockRaw>> Callee { get; set; }
        public MockClient<IWampClient<MockRaw>> Caller { get; set; }

        public DealerCall Call { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Registrations { get; set; }

        public MockBinding Binding
        {
            get { return mBinding; }
        }

        public WampMockClientBuilder<MockRaw> ClientBuilder
        {
            get { return mClientBuilder; }
        }

        public WampRpcServer<MockRaw> Server
        {
            get { return mServer; }
        }

        public WampIncomingMessageHandler<MockRaw, IWampClient<MockRaw>> Handler
        {
            get { return mHandler; }
        }
    }
}