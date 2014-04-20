using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Tests.Wampv2.Binding
{
    public class MockBinding : WampBinding<MockRaw>
    {
        public MockBinding() : base("mock", new MockRawFormatter())
        {
        }

        public override WampMessage<MockRaw> GetRawMessage(WampMessage<MockRaw> message)
        {
            return new MockWampMessage(message);
        }
    }
}