using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
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
            (IWampRawRpcOperationCallback caller,
             IWampFormatter<TMessage> formatter,
             TMessage options,
             TMessage[] arguments,
             IDictionary<string, TMessage> argumentsKeywords);

#if NET45

        protected async override void InnerInvoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {
                Task<object> task =
                    InvokeAsync(caller,
                                formatter,
                                options,
                                arguments,
                                argumentsKeywords);

                object result = await task;

                CallResult(caller, result, null);
            }
            catch (WampException ex)
            {
                IWampErrorCallback callback = new WampRpcErrorCallback(caller);
                callback.Error(ex);
            }
            catch (Exception ex)
            {
                WampRpcRuntimeException wampException = ConvertExceptionToRuntimeException(ex);
                IWampErrorCallback callback = new WampRpcErrorCallback(caller);
                callback.Error(wampException);
            }
        }

#elif NET40

        protected override void InnerInvoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {
                Task<object> task =
                    InvokeAsync(caller,
                               formatter,
                               options,
                               arguments,
                               argumentsKeywords);

                task.ContinueWith(x => TaskCallback(x, caller));
            }
            catch (WampException ex)
            {
                IWampErrorCallback callback = new WampRpcErrorCallback(caller);
                callback.Error(ex);
            }
        }

        private void TaskCallback(Task<object> task, IWampRawRpcOperationCallback caller)
        {
            if (task.Exception == null)
            {
                object result = task.Result;
                CallResult(caller, result, null);
            }
            else
            {
                Exception innerException = task.Exception.InnerException;

                WampException wampException = innerException as WampException;

                if (wampException == null)
                {
                    wampException = ConvertExceptionToRuntimeException(innerException);
                }

                IWampErrorCallback callback = new WampRpcErrorCallback(caller);
                callback.Error(wampException);
            }
        }

#endif
    }
}