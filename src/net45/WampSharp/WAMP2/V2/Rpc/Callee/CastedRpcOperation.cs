using WampSharp.Core.Serialization;
using WampSharp.V2.Client;

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

        public void Invoke(IWampRawRpcOperationCallback caller, TMessage details)
        {
            mOperation.Invoke(caller, mFormatter, details);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, TMessage details, TMessage[] arguments)
        {
            mOperation.Invoke(caller, mFormatter, details, arguments);
        }

        public void Invoke(IWampRawRpcOperationCallback caller, TMessage details, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mOperation.Invoke(caller, mFormatter, details, arguments, argumentsKeywords);
        }
    }
}