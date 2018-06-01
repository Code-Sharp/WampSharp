using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    internal class MockRawCallback : IWampRawRpcOperationClientCallback
    {
        private IDictionary<string, ISerializedValue> mArgumentsKeywords;

        public ResultDetails Details { get; private set; }

        public IEnumerable<ISerializedValue> Arguments { get; private set; }

        public IDictionary<string, ISerializedValue> ArgumentsKeywords => mArgumentsKeywords;

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
        {
            Details = details;
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
        {
            Details = details;
            Arguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x))
                                  .ToArray();
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            Details = details;
            Arguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x))
                                  .ToArray();
            mArgumentsKeywords = argumentsKeywords.ToDictionary(x => x.Key,
                                                                x => (ISerializedValue)new SerializedValue<TMessage>(formatter, x.Value));
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
        {
            throw new System.NotImplementedException();
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
        {
            throw new System.NotImplementedException();
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            throw new System.NotImplementedException();
        }
    }
}