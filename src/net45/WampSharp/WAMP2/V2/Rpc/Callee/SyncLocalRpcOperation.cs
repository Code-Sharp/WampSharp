using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
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

        protected override void InnerInvoke<TMessage>(IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            try
            {
                IDictionary<string, object> outputs;

                object result =
                    InvokeSync(caller,
                               formatter,
                               options,
                               arguments,
                               argumentsKeywords,
                               out outputs);

                CallResult(caller, result, outputs);
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

        protected abstract object InvokeSync<TMessage>
            (IWampRawRpcOperationRouterCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords, out IDictionary<string, object> outputs);
    }
}