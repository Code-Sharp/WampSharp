using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class DealerScenario : BaseScenario
    {
        public DealerCall Call { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Registrations { get; set; }

        public MockClient<IWampClientProxy<MockRaw>> Callee { get; set; }
        public MockClient<IWampClientProxy<MockRaw>> Caller { get; set; }

        protected override object CreateServer()
        {
            return new WampRpcServer<MockRaw>
                (new WampRpcOperationCatalog(),
                 Binding,
                 new LooseUriValidator());
        }
    }
}