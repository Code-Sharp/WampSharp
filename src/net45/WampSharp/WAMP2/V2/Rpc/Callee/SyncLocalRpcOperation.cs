using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
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

        protected override void InnerInvoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
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
        }

        protected abstract object InvokeSync<TMessage>
            (IWampRawRpcOperationCallback caller,
             IWampFormatter<TMessage> formatter,
             TMessage options,
             TMessage[] arguments,
             IDictionary<string, TMessage> argumentsKeywords,
             out IDictionary<string, object> outputs);

        private class WampRpcErrorCallback : IWampErrorCallback
        {
            private readonly IWampRawRpcOperationCallback mCallback;

            public WampRpcErrorCallback(IWampRawRpcOperationCallback callback)
            {
                mCallback = callback;
            }

            public void Error(object details, string error)
            {
                mCallback.Error(ObjectFormatter, details, error);
            }

            public void Error(object details, string error, object[] arguments)
            {
                mCallback.Error(ObjectFormatter, details, error, arguments);
            }

            public void Error(object details, string error, object[] arguments, object argumentsKeywords)
            {
                mCallback.Error(ObjectFormatter, details, error, arguments, argumentsKeywords);
            }
        }
    }
}