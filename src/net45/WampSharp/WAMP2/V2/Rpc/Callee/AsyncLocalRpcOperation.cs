using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Logging;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Error;

namespace WampSharp.V2.Rpc
{
    public abstract class AsyncLocalRpcOperation: LocalRpcOperation
    {
        protected AsyncLocalRpcOperation(string procedure) : base(procedure)
        {
        }

        protected abstract Task<object> InvokeAsync<TMessage>
            (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        protected override async void InnerInvoke<TMessage>(IWampRawRpcOperationRouterCallback caller,
                                                            IWampFormatter<TMessage> formatter,
                                                            InvocationDetails details,
                                                            TMessage[] arguments,
                                                            IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {
                Task<object> task =
                    InvokeAsync(caller,
                                formatter,
                                details,
                                arguments,
                                argumentsKeywords);

                object result = await task;

                CallResult(caller, result);
            }
            catch (Exception ex)
            {
                mLogger.ErrorFormat(ex, "An error occured while calling {ProcedureUri}", this.Procedure);

                WampException wampException = ex as WampException;

                if (wampException == null)
                {
                    wampException = ConvertExceptionToRuntimeException(ex);
                }

                IWampErrorCallback callback = new WampRpcErrorCallback(caller);

                callback.Error(wampException);
            }
        }




        protected void CallResult(IWampRawRpcOperationRouterCallback caller, object result, YieldOptions yieldOptions = null)
        {
            yieldOptions = yieldOptions ?? new YieldOptions();

            object[] resultArguments = GetResultArguments(result);

            IDictionary<string, object> resultArgumentKeywords =
                GetResultArgumentKeywords(result);

            CallResult(caller,
                       yieldOptions,
                       resultArguments,
                       resultArgumentKeywords);
        }

        protected virtual IDictionary<string, object> GetResultArgumentKeywords(object result)
        {
            return null;
        }
    }
}