using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.Logging;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Error;

namespace WampSharp.V2.Rpc
{
    public abstract class SyncLocalRpcOperation: LocalRpcOperation
    {
        protected SyncLocalRpcOperation(string procedure)
            : base(procedure)
        {
        }

        public override bool SupportsCancellation
        {
            get
            {
                return false;
            }
        }

        protected override IWampCancellableInvocation InnerInvoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {
                IDictionary<string, object> outputs;

                object result =
                    InvokeSync(caller,
                               formatter,
                               details,
                               arguments,
                               argumentsKeywords,
                               out outputs);

                CallResult(caller, result, outputs);
            }
            catch (WampException ex)
            {
                mLogger.ErrorFormat(ex, "An error occured while calling {ProcedureUri}", this.Procedure);
                IWampErrorCallback callback = new WampRpcErrorCallback(caller);
                callback.Error(ex);
            }
            catch (Exception ex)
            {
                WampException wampException = ConvertExceptionToRuntimeException(ex);
                IWampErrorCallback callback = new WampRpcErrorCallback(caller);
                callback.Error(wampException);
            }

            return null;
        }

        protected void CallResult(IWampRawRpcOperationRouterCallback caller, object result, IDictionary<string, object> outputs, YieldOptions yieldOptions = null)
        {
            yieldOptions = yieldOptions ?? new YieldOptions();
            object[] resultArguments = GetResultArguments(result);

            IDictionary<string, object> argumentKeywords = 
                GetResultArgumentKeywords(result, outputs);

            CallResult(caller,
                       yieldOptions,
                       resultArguments,
                       argumentKeywords);
        }

        protected virtual IDictionary<string, object> GetResultArgumentKeywords(object result, IDictionary<string, object> outputs)
        {
            return outputs;
        }

        protected abstract object InvokeSync<TMessage>
            (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords, out IDictionary<string, object> outputs);
    }
}