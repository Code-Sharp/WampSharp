using System.Linq;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.Dealer
{
    public class Call
    {
        private long mRequestId;
        private MockRaw mOptions;
        private string mProcedure;
        private MockRaw[] mArguments;
        private MockRaw mArgumentsKeywords;

        public Call(long requestId, MockRaw options, string procedure, MockRaw[] arguments, MockRaw argumentsKeywords)
        {
            mRequestId = requestId;
            mOptions = options;
            mProcedure = procedure;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public Call(WampMessage<MockRaw> message)
        {
            mRequestId = (long) message.Arguments[0].Value;
            mOptions = message.Arguments[1];
            mProcedure = (string) message.Arguments[2].Value;

            if (message.Arguments.Length >= 4)
            {
                mArguments =
                    (message.Arguments[3].Value as object[])
                        .Select(x => new MockRaw(x)).ToArray();

            }

            if (message.Arguments.Length >= 5)
            {
                mArgumentsKeywords = message.Arguments[4];
            }
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

        public MockRaw[] Arguments
        {
            get { return mArguments; }
        }

        public MockRaw ArgumentsKeywords
        {
            get { return mArgumentsKeywords; }
        }
    }
}