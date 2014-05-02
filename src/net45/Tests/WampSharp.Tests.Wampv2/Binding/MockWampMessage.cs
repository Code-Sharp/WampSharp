using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.Binding
{
    public class MockWampMessage : WampMessage<MockRaw>
    {
        public MockWampMessage()
        {
        }

        public MockWampMessage(WampMessage<MockRaw> other)
            : base(other)
        {
        }

        public override string ToString()
        {
            // Maybe use a debugger display instead.
            return WampMessagePrinter.ToString(this);
        }
    }
}