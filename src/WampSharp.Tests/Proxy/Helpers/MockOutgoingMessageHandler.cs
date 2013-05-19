using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Proxy.Helpers
{
    public class MockOutgoingMessageHandler : IWampOutgoingMessageHandler<MockRaw>
    {
        public WampMessage<MockRaw> Message { get; set; }

        public void Handle(WampMessage<MockRaw> message)
        {
            Message = message;
        }
    }
}