using WampSharp.V2.Client;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcInvocation<TMessage>
    {
        private readonly IWampRpcOperation mOperation;
        private readonly IWampRawRpcOperationCallback mCallback;
        private readonly object mOptions;
        private readonly object[] mArguments;
        private readonly object mArgumentsKeywords;

        public WampRpcInvocation(IWampRpcOperation operation, IWampRawRpcOperationCallback callback, object options, object[] arguments, object argumentsKeywords)
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

        public object Options
        {
            get
            {
                return mOptions;
            }
        }

        public object[] Arguments
        {
            get
            {
                return mArguments;
            }
        }

        public object ArgumentsKeywords
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