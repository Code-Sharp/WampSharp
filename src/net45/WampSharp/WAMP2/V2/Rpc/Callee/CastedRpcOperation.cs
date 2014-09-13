using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class CastedRpcOperation<TMessage> : IWampRpcOperation<TMessage>
    {
        private readonly IWampRpcOperation mOperation;
        private readonly IWampFormatter<TMessage> mFormatter;

        public CastedRpcOperation(IWampRpcOperation operation, IWampFormatter<TMessage> formatter)
        {
            mOperation = operation;
            mFormatter = formatter;
        }

        public string Procedure
        {
            get
            {
                return mOperation.Procedure;
            }
        }

        public void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details)
        {
            mOperation.Invoke(caller, mFormatter, details);
        }

        public void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details, TMessage[] arguments)
        {
            mOperation.Invoke(caller, mFormatter, details, arguments);
        }

        public void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mOperation.Invoke(caller, mFormatter, details, arguments, argumentsKeywords);
        }
    }
}