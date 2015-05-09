using System.Collections.Generic;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcInvocation
    {
        private readonly RemoteWampCalleeDetails mOperation;
        private readonly IWampRawRpcOperationRouterCallback mCallback;
        private readonly InvocationDetails mOptions;
        private readonly object[] mArguments;
        private readonly IDictionary<string, object> mArgumentsKeywords;

        public WampRpcInvocation(RemoteWampCalleeDetails operation, IWampRawRpcOperationRouterCallback callback, InvocationDetails options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mOperation = operation;
            mCallback = callback;
            mOptions = options;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public string Procedure
        {
            get
            {
                return mOperation.Procedure;
            }
        }

        public RemoteWampCalleeDetails Operation
        {
            get
            {
                return mOperation;
            }
        }

        public IWampRawRpcOperationRouterCallback Callback
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