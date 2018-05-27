using System.Collections.Generic;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    internal class WampRpcInvocation
    {
        private readonly IDictionary<string, object> mArgumentsKeywords;

        public WampRpcInvocation(RemoteWampCalleeDetails operation, IWampRawRpcOperationRouterCallback callback, InvocationDetails options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            Operation = operation;
            Callback = callback;
            Options = options;
            Arguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public string Procedure
        {
            get
            {
                return Operation.Procedure;
            }
        }

        public RemoteWampCalleeDetails Operation { get; }

        public IWampRawRpcOperationRouterCallback Callback { get; }

        public InvocationDetails Options { get; }

        public object[] Arguments { get; }

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