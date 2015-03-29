using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
#if !PCL
    [Serializable]
#endif
    public class WampRpcRuntimeException : WampException
    {
        private const string ErrorUri = "wamp.error.runtime_error";

        public WampRpcRuntimeException(params object[] arguments)
            : base(ErrorUri, arguments)
        {
        }

        public WampRpcRuntimeException(object[] arguments, IDictionary<string, object> argumentsKeywords) :
            base(ErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcRuntimeException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords)
            : base(details, ErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcRuntimeException(IDictionary<string, object> details, string message,
                                       IDictionary<string, object> argumentsKeywords) :
                                           base(details, ErrorUri, message, argumentsKeywords)
        {
        }

        public WampRpcRuntimeException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords, string message, Exception inner) :
                                           base(details, ErrorUri, arguments, argumentsKeywords, message, inner)
        {
        }

#if !PCL
        protected WampRpcRuntimeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}