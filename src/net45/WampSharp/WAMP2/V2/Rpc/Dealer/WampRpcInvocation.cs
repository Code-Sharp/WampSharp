using WampSharp.V2.Client;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcInvocation<TMessage>
    {
        private readonly IWampRpcOperation mOperation;
        private readonly IWampRawRpcOperationCallback mCallback;
        private readonly TMessage mOptions;
        private readonly TMessage[] mArguments;
        private readonly TMessage mArgumentsKeywords;

        public WampRpcInvocation(IWampRpcOperation operation, IWampRawRpcOperationCallback callback, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mOperation = operation;
            mCallback = callback;
            mOptions = options;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public IWampRpcOperation Operation
        {
            get
            {
                return mOperation;
            }
        }

        public IWampRawRpcOperationCallback Callback
        {
            get
            {
                return mCallback;
            }
        }

        public TMessage Options
        {
            get
            {
                return mOptions;
            }
        }

        public TMessage[] Arguments
        {
            get
            {
                return mArguments;
            }
        }

        public TMessage ArgumentsKeywords
        {
            get
            {
                return mArgumentsKeywords;
            }
        }

        public long InvocationId
        {
            get; 
            set;
        }
    }
}