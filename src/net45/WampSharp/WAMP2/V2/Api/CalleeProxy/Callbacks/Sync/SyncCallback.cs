using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using WampSharp.Core.Logs;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCallback : SyncCallbackBase
    {
        private readonly ILogger mLogger;
        private readonly IOperationResultExtractor mExtractor;
        protected readonly MethodInfoHelper mMethodInfoHelper;
        private readonly object[] mArguments;
        private object mResult;

        public SyncCallback(string procedureUri, MethodInfoHelper methodInfoHelper, object[] arguments, IOperationResultExtractor extractor)
        {
            mMethodInfoHelper = methodInfoHelper;
            mLogger = WampLoggerFactory.Create(typeof (SyncCallback) + "." + procedureUri);
            mArguments = arguments;
            mExtractor = extractor;
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

            try
            {
                mMethodInfoHelper.PopulateOutOrRefValues
                    (formatter,
                     mArguments,
                     outOrRefParameters);

                SetResult(formatter, arguments);
            }
            catch (Exception ex)
            {
                mLogger.Error("An error occured while trying to populate out/ref parameters", ex);
                SetException(ex);
            }
        }

        private void SetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            object result = mExtractor.GetResult(formatter, arguments);
            SetResult(result);
        }

        protected void SetResult(object result)
        {
            mResult = result;
            ResultArrived();
        }
    }
}