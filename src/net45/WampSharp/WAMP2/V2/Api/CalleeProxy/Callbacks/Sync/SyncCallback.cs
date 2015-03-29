using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCallback<TResult> : SyncCallbackBase
    {
        private readonly IOperationResultExtractor<TResult> mExtractor;
        protected readonly MethodInfoHelper mMethodInfoHelper;
        private readonly object[] mArguments;
        private TResult mResult;

        public SyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments, IOperationResultExtractor<TResult> extractor)
        {
            mMethodInfoHelper = methodInfoHelper;
            mArguments = arguments;
            mExtractor = extractor;
        }

        public TResult OperationResult
        {
            get { return mResult; }
        }

        public override void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
        {
            // TODO: throw exception if not nullable.
            SetResult(default(TResult));
        }

        public override void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments)
        {
            SetResult(formatter, arguments);
        }

        public override void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            IDictionary<string, TMessage> outOrRefParameters = argumentsKeywords;

            mMethodInfoHelper.PopulateOutOrRefValues
                (formatter,
                 mArguments,
                 outOrRefParameters);

            SetResult(formatter, arguments);
        }

        private void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            TResult result = mExtractor.GetResult(formatter, arguments);
            SetResult(result);
        }

        protected void SetResult(TResult result)
        {
            mResult = result;
            ResultArrived();
        }
    }
}