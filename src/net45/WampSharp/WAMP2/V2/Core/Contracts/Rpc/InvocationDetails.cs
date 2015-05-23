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
            Procedure = details.Procedure;
            OriginalValue = details.OriginalValue;
        }

        [DataMember(Name = "timeout")]
        internal bool? Timeout { get; set; }

        [DataMember(Name = "receive_progress")]
        public bool? ReceiveProgress { get; set; }

        [DataMember(Name = "caller")]
        public long? Caller { get; set; }

        [DataMember(Name = "procedure")]
        public string Procedure { get; set; }
    }
}