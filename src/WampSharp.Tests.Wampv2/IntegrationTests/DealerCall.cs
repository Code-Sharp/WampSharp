using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.IntegrationTests
{
    internal class DealerCall
    {
        public WampMessage<MockRaw> Request { get; set; }
        public IEnumerable<WampMessage<MockRaw>> Responses { get; set; }
    }
}