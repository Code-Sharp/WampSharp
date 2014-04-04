namespace WampSharp.V2.Rpc
{
    internal class WampRpcInvocation<TMessage>
    {
        private readonly IWampRpcOperationCallback mCaller;
        private readonly TMessage mOptions;
        private readonly TMessage[] mArguments;
        private readonly TMessage mArgumentsKeywords;

        public WampRpcInvocation(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords)
        {
            mCaller = caller;
            mOptions = options;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public IWampRpcOperationCallback Caller
        {
            get
            {
                return mCaller;
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