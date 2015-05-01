using System.Collections.Generic;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
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

        [DataMember(Name = "timeout")]
        internal bool? Timeout { get; set; }

        [DataMember(Name = "receive_progress")]
        public bool? ReceiveProgress { get; set; }

        [DataMember(Name = "caller")]
        public long? Caller { get; set; }

        [DataMember(Name = "caller_transport")]
        internal IDictionary<string, object> CallerTransport { get; set; }

        [DataMember(Name = "authid")]
        internal string AuthId { get; set; }

        [DataMember(Name = "authrole")]
        internal string AuthRole { get; set; }

        [DataMember(Name = "authmethod")]
        internal string AuthMethod { get; set; }
    }
}