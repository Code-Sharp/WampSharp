using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcCanceledException : WampException
    {
        private const string ErrorUri = WampErrors.Canceled;

        public WampRpcCanceledException(params object[] arguments)
            : base(ErrorUri, arguments)
        {
        }

        public WampRpcCanceledException(object[] arguments, IDictionary<string, object> argumentsKeywords) :
            base(ErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcCanceledException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords)
            : base(details, ErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcCanceledException(IDictionary<string, object> details, string message,
                                       IDictionary<string, object> argumentsKeywords) :
            base(details, ErrorUri, message, argumentsKeywords)
        {
        }

        public WampRpcCanceledException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords, string message, Exception inner) :
            base(details, ErrorUri, arguments, argumentsKeywords, message, inner)
        {
        }

#if !PCL
        protected WampRpcCanceledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }

}