using System.Collections.Generic;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Invocation)]
    public class InvocationDetails : WampDetailsOptions
    {
        public InvocationDetails()
        {
        }

        public InvocationDetails(InvocationDetails details)
        {
            Timeout = details.Timeout;
            ReceiveProgress = details.ReceiveProgress;
            Caller = details.Caller;
            CallerTransport = details.CallerTransport;
            AuthId = details.AuthId;
            AuthRole = details.AuthRole;
            AuthMethod = details.AuthMethod;
            OriginalValue = details.OriginalValue;
        }

        [PropertyName("timeout")]
        internal bool? Timeout { get; set; }

        [PropertyName("receive_progress")]
        public bool? ReceiveProgress { get; set; }

        [PropertyName("caller")]
        public long? Caller { get; set; }

        [PropertyName("caller_transport")]
        internal IDictionary<string, object> CallerTransport { get; set; }

        [PropertyName("authid")]
        internal string AuthId { get; set; }

        [PropertyName("authrole")]
        internal string AuthRole { get; set; }

        [PropertyName("authmethod")]
        internal string AuthMethod { get; set; }
    }
}