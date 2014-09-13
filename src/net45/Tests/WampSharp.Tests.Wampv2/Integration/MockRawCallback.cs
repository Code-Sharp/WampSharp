using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    internal class MockRawCallback : IWampClientRawRpcOperationCallback
    {
        private ResultDetails mDetails;
        private IEnumerable<ISerializedValue> mArguments;
        private IDictionary<string, ISerializedValue> mArgumentsKeywords;

        public ResultDetails Details
        {
            get { return mDetails; }
        }

        public IEnumerable<ISerializedValue> Arguments
        {
            get { return mArguments; }
        }

        public IDictionary<string, ISerializedValue> ArgumentsKeywords
        {
            get { return mArgumentsKeywords; }
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
        {
            mDetails = details;
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
        {
            mDetails = details;
            mArguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x))
                                  .ToArray();
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mDetails = details;
            mArguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x))
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