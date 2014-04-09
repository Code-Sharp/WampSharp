using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2.Binding
{
    public class MockWampMessage<TMessage> : WampMessage<TMessage>
    {
        public MockWampMessage()
        {
        }

        public MockWampMessage(WampMessage<TMessage> other) : base(other)
        {
        }
    }
}