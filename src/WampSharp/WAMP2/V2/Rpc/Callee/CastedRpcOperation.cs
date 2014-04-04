using WampSharp.Core.Serialization;

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

        public void Invoke(IWampRpcOperationCallback caller, TMessage options)
        {
            mOperation.Invoke(caller, mFormatter, options);
        }

        public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments)
        {
            mOperation.Invoke(caller, mFormatter, options, arguments);
        }

        public void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mOperation.Invoke(caller, mFormatter, options, arguments, argumentsKeywords);
        }
    }
}