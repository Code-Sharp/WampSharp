using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;
using MockRawFormatter = WampSharp.Tests.Wampv2.TestHelpers.MockRawFormatter;

namespace WampSharp.Tests.Wampv2.Dealer
{
    public class Registration
    {
        private readonly string mProcedure;

        public Registration(long requestId, RegisterOptions options, string procedure)
        {
            RequestId = requestId;
            Options = options;
            mProcedure = procedure;
        }

        public Registration(WampMessage<MockRaw> message)
        {
            RequestId = (long) message.Arguments[0].Value;
            MockRawFormatter formatter = new MockRawFormatter();

            Options = formatter.Deserialize<RegisterOptions>
                (formatter.Serialize(message.Arguments[1].Value));

            mProcedure = (string)message.Arguments[2].Value;
        }

        public long RequestId { get; }

        public RegisterOptions Options { get; }

        public string Procedure => mProcedure;
    }
}