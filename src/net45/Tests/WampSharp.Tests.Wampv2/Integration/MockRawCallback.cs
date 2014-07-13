using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    internal class MockRawCallback : IWampRawRpcOperationCallback
    {
        private ISerializedValue mDetails;
        private IEnumerable<ISerializedValue> mArguments;
        private ISerializedValue mArgumentsKeywords;

        public ISerializedValue Details
        {
            get { return mDetails; }
        }

        public IEnumerable<ISerializedValue> Arguments
        {
            get { return mArguments; }
        }

        public ISerializedValue ArgumentsKeywords
        {
            get { return mArgumentsKeywords; }
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details)
        {
            mDetails = new SerializedValue<TMessage>(formatter, details);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments)
        {
            mDetails = new SerializedValue<TMessage>(formatter, details);
            mArguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x))
                                  .ToArray();
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mDetails = new SerializedValue<TMessage>(formatter, details);
            mArguments = arguments.Select(x => new SerializedValue<TMessage>(formatter, x))
                                  .ToArray();
            mArgumentsKeywords = new SerializedValue<TMessage>(formatter, details);
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