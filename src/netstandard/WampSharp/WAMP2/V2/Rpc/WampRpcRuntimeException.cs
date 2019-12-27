using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    [Serializable]
    public class WampRpcRuntimeException : WampException
    {
        private const string RuntimeErrorUri = "wamp.error.runtime_error";

        public WampRpcRuntimeException(params object[] arguments)
            : base(RuntimeErrorUri, arguments)
        {
        }

        public WampRpcRuntimeException(object[] arguments, IDictionary<string, object> argumentsKeywords) :
            base(RuntimeErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcRuntimeException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords)
            : base(details, RuntimeErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcRuntimeException(IDictionary<string, object> details, string message,
                                       IDictionary<string, object> argumentsKeywords) :
                                           base(details, RuntimeErrorUri, message, argumentsKeywords)
        {
        }

        public WampRpcRuntimeException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords, string message, Exception inner) :
                                           base(details, RuntimeErrorUri, arguments, argumentsKeywords, message, inner)
        {
        }

        protected WampRpcRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}