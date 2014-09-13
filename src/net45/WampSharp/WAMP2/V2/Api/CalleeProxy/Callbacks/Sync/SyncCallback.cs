using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class SyncCallback : SyncCallbackBase
    {
        protected readonly MethodInfoHelper mMethodInfoHelper;
        private readonly object[] mArguments;
        private object mResult;

        public SyncCallback(MethodInfoHelper methodInfoHelper, object[] arguments)
        {
            mMethodInfoHelper = methodInfoHelper;
            mArguments = arguments;
        }

        public object OperationResult
        {
            get { return mResult; }
        }

        public override void Result<TMessage>(IWampFormatter<TMessage> formatter, ResultDetails details)
        {
            SetResult(null);
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

        protected abstract void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments);

        protected void SetResult(object result)
        {
            mResult = result;
            ResultArrived();
        }
    }
}