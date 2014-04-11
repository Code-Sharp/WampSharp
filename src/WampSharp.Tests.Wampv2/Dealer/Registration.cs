using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2
{
    public class Registration
    {
        private long mRequestId;
        private MockRaw mOptions;
        private string mProcedure;

        public Registration(long requestId, MockRaw options, string procedure)
        {
            mRequestId = requestId;
            mOptions = options;
            mProcedure = procedure;
        }

        public Registration(WampMessage<MockRaw> message)
        {
            mRequestId = (long) message.Arguments[0].Value;
            mOptions = message.Arguments[1];
            mProcedure = (string) message.Arguments[2].Value;
        }

        public long RequestId
        {
            get { return mRequestId; }
        }

        public MockRaw Options
        {
            get { return mOptions; }
        }

        public string Procedure
        {
            get { return mProcedure; }
        }
    }
}