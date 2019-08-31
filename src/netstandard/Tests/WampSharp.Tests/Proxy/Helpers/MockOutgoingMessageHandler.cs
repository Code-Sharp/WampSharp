using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Proxy.Helpers
{
    public class MockOutgoingMessageHandler : IWampOutgoingMessageHandler
    {
        private readonly IWampFormatter<MockRaw> mFormatter;
        public WampMessage<MockRaw> Message { get; set; }

        public MockOutgoingMessageHandler(IWampFormatter<MockRaw> formatter)
        {
            mFormatter = formatter;
        }

        public void Handle(WampMessage<object> message)
        {
            Message = mFormatter.SerializeMessage(message);
        }
    }
}