using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampClientRouterCallbackAdapter : IWampRawRpcOperationRouterCallback
    {
        private readonly IWampRawRpcOperationClientCallback mCaller;
        private readonly InvocationDetails mOptions;

        public WampClientRouterCallbackAdapter(IWampRawRpcOperationClientCallback caller, InvocationDetails options)
        {
            mCaller = caller;
            mOptions = options;
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            mCaller.Result(formatter, resultDetails);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details, TMessage[] arguments)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            mCaller.Result(formatter, resultDetails, arguments);
        }

        public void Result<TMessage>(IWampFormatter<TMessage> formatter, YieldOptions details, TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentsKeywords)
        {
            ResultDetails resultDetails = GetResultDetails(details);
            mCaller.Result(formatter, resultDetails, arguments, argumentsKeywords);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
        {
            mCaller.Error(formatter, details, error);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
        {
            mCaller.Error(formatter, details, error, arguments);
        }

        public void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            mCaller.Error(formatter, details, error, arguments, argumentsKeywords);
        }

        private ResultDetails GetResultDetails(YieldOptions details)
        {
            return new ResultDetails {Progress = details.Progress};
        }
    }
}