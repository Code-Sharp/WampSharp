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

        public override bool SupportsCancellation => false;

        protected override IWampCancellableInvocation InnerInvoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {

                object result =
                    InvokeSync(caller,
                               formatter,
                               details,
                               arguments,
                               argumentsKeywords,
                               out IDictionary<string, object> outputs);

                return OnResult(caller, result, outputs);
            }
            catch (WampException ex)
            {
                HandleException(caller, ex);
            }
            catch (Exception ex)
            {
                HandleException(caller, ex);
            }

            return null;
        }

        protected void HandleException(IWampRawRpcOperationRouterCallback caller, WampException ex)
        {
            mLogger.ErrorFormat(ex, "An error occurred while calling {ProcedureUri}", this.Procedure);
            IWampErrorCallback callback = new WampRpcErrorCallback(caller);
            callback.Error(ex);
        }

        protected void HandleException(IWampRawRpcOperationRouterCallback caller, Exception ex)
        {
            WampException wampException = ConvertExceptionToRuntimeException(ex);
            IWampErrorCallback callback = new WampRpcErrorCallback(caller);
            callback.Error(wampException);
        }

        protected virtual IWampCancellableInvocation OnResult(IWampRawRpcOperationRouterCallback caller, object result, IDictionary<string, object> outputs)
        {
            CallResult(caller, result, outputs);
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
