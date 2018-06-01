using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Binding;
using MockRawFormatter = WampSharp.Tests.Wampv2.TestHelpers.MockRawFormatter;

namespace WampSharp.Tests.Wampv2.Binding
{
    public class MockBinding : WampBinding<MockRaw>
    {
        public MockBinding() : base("mock", new MockRawFormatter())
        {
        }

        public override WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return message;
        }
    }
}