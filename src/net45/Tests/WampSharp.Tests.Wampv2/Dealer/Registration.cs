using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.Dealer
{
    public class Registration
    {
        private long mRequestId;
        private RegisterOptions mOptions;
        private string mProcedure;

        public Registration(long requestId, RegisterOptions options, string procedure)
        {
            mRequestId = requestId;
            mOptions = options;
            mProcedure = procedure;
        }

        public Registration(WampMessage<MockRaw> message)
        {
            mRequestId = (long) message.Arguments[0].Value;
            MockRawFormatter formatter = new MockRawFormatter();

            mOptions = formatter.Deserialize<RegisterOptions>
                (formatter.Serialize(message.Arguments[1].Value));

            mProcedure = (string)message.Arguments[2].Value;
        }

        public long RequestId
        {
            get { return mRequestId; }
        }

        public RegisterOptions Options
        {
            get { return mOptions; }
        }

        public string Procedure
        {
            get { return mProcedure; }
        }
    }
}