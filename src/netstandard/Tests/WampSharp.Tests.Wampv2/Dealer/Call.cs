using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;
using MockRawFormatter = WampSharp.Tests.Wampv2.TestHelpers.MockRawFormatter;

namespace WampSharp.Tests.Wampv2.Dealer
{
    public class Call
    {
        private readonly IDictionary<string, MockRaw> mArgumentsKeywords;

        public Call(long requestId, CallOptions options, string procedure, MockRaw[] arguments, IDictionary<string, MockRaw> argumentsKeywords)
        {
            RequestId = requestId;
            Options = options;
            Procedure = procedure;
            Arguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public Call(WampMessage<MockRaw> message)
        {
            RequestId = (long) message.Arguments[0].Value;
            MockRawFormatter formatter = new MockRawFormatter();
            
            Options = formatter.Deserialize<CallOptions>
                (formatter.Serialize(message.Arguments[1].Value));
            
            Procedure = (string) message.Arguments[2].Value;

            if (message.Arguments.Length >= 4)
            {
                Arguments =
                    (message.Arguments[3].Value as object[])
                        .Select(x => new MockRaw(x)).ToArray();

            }

            if (message.Arguments.Length >= 5)
            {
                //mArgumentsKeywords = message.Arguments[4];
                mArgumentsKeywords = new Dictionary<string, MockRaw>();
            }
        }

        public long RequestId { get; }

        public CallOptions Options { get; }

        public string Procedure { get; }

        public MockRaw[] Arguments { get; }

        public IDictionary<string, MockRaw> ArgumentsKeywords => mArgumentsKeywords;
    }
}