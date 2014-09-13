using System.Collections.Generic;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcInvocation<TMessage>
    {
        private readonly IWampRpcOperation mOperation;
        private readonly IWampRouterRawRpcOperationCallback mCallback;
        private readonly InvocationDetails mOptions;
        private readonly object[] mArguments;
        private readonly IDictionary<string, object> mArgumentsKeywords;

        public WampRpcInvocation(IWampRpcOperation operation, IWampRouterRawRpcOperationCallback callback, InvocationDetails options, object[] arguments, IDictionary<string, object> argumentsKeywords)
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

        public IWampRouterRawRpcOperationCallback Callback
        {
            get
            {
                return mCallback;
            }
        }

        public InvocationDetails Options
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

        public IDictionary<string, object> ArgumentsKeywords
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