using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.Dealer
{
    public class Call
    {
        private long mRequestId;
        private CallOptions mOptions;
        private string mProcedure;
        private MockRaw[] mArguments;
        private IDictionary<string, MockRaw> mArgumentsKeywords;

        public Call(long requestId, CallOptions options, string procedure, MockRaw[] arguments, IDictionary<string, MockRaw> argumentsKeywords)
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
            MockRawFormatter formatter = new MockRawFormatter();
            
            mOptions = formatter.Deserialize<CallOptions>
                (formatter.Serialize(message.Arguments[1].Value));
            
            mProcedure = (string) message.Arguments[2].Value;

            if (message.Arguments.Length >= 4)
            {
                mArguments =
                    (message.Arguments[3].Value as object[])
                        .Select(x => new MockRaw(x)).ToArray();

            }

            if (message.Arguments.Length >= 5)
            {
                //mArgumentsKeywords = message.Arguments[4];
                mArgumentsKeywords = new Dictionary<string, MockRaw>();
            }
        }

        public long RequestId
        {
            get { return mRequestId; }
        }

        public CallOptions Options
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

        public IDictionary<string, MockRaw> ArgumentsKeywords
        {
            get { return mArgumentsKeywords; }
        }
    }
}