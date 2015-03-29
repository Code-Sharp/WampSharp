using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Binding;

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