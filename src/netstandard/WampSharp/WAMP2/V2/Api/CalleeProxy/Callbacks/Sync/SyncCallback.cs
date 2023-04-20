using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.Logging;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCallback<TResult> : SyncCallbackBase
    {
        private readonly IOperationResultExtractor<TResult> mResultExtractor;
        private readonly ILog mLogger;
        protected readonly MethodInfoHelper mMethodInfoHelper;
        private readonly object[] mArguments;

        public SyncCallback(string procedureUri, MethodInfoHelper methodInfoHelper, object[] arguments, IOperationResultExtractor<TResult> resultExtractor)
        {
            mMethodInfoHelper = methodInfoHelper;
            mLogger = LogProvider.GetLogger(typeof (SyncCallback<TResult>) + "." + procedureUri);
            mArguments = arguments;
            mResultExtractor = resultExtractor;
        }

        public TResult OperationResult { get; private set; }

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

            try
            {
                mMethodInfoHelper.PopulateOutOrRefValues
                    (formatter,
                     mArguments,
                     outOrRefParameters);

                SetResult(formatter, arguments, argumentsKeywords);
            }
            catch (Exception ex)
            {
                mLogger.Error("An error occurred while trying to populate out/ref parameters", ex);
                SetException(ex);
            }
        }

        private void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords = null)
        {
            try
            {
                TResult result = mResultExtractor.GetResult(formatter, arguments, argumentKeywords);
                SetResult(result);
            }
            catch (Exception ex)
            {
                SetException(ex);
            }
        }

        protected void SetResult(TResult result)
        {
            OperationResult = result;
            ResultArrived();
        }
    }
}