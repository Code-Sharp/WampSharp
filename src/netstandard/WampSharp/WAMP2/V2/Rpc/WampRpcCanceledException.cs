using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public class WampRpcCanceledException : WampException
    {
        private const string CancellationErrorUri = WampErrors.Canceled;

        public WampRpcCanceledException(params object[] arguments)
            : base(CancellationErrorUri, arguments)
        {
        }

        public WampRpcCanceledException(object[] arguments, IDictionary<string, object> argumentsKeywords) :
            base(CancellationErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcCanceledException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords)
            : base(details, CancellationErrorUri, arguments, argumentsKeywords)
        {
        }

        public WampRpcCanceledException(IDictionary<string, object> details, string message,
                                       IDictionary<string, object> argumentsKeywords) :
            base(details, CancellationErrorUri, message, argumentsKeywords)
        {
        }

        public WampRpcCanceledException(IDictionary<string, object> details, object[] arguments,
                                       IDictionary<string, object> argumentsKeywords, string message, Exception inner) :
            base(details, CancellationErrorUri, arguments, argumentsKeywords, message, inner)
        {
        }

        protected WampRpcCanceledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}