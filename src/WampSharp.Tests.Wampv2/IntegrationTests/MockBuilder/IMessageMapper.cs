using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public interface IMessageMapper
    {
        WampMessage<MockRaw> MapRequest(WampMessage<MockRaw> message, IEnumerable<WampMessage<MockRaw>> messages, bool ignoreRequestId);
    }
}