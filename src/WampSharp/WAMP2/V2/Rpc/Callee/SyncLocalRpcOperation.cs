using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public abstract class SyncLocalRpcOperation: LocalRpcOperation
    {
        protected SyncLocalRpcOperation(string procedure)
            : base(procedure)
        {
        }

        protected override void InnerInvoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
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
                caller.Error(ex.Details, ex.ErrorUri);
            }
        }

        protected abstract object InvokeSync<TMessage>
            (IWampRpcOperationCallback caller,
             IWampFormatter<TMessage> formatter,
             TMessage options,
             TMessage[] arguments,
             IDictionary<string, TMessage> argumentsKeywords,
             out IDictionary<string, object> outputs);
    }
}