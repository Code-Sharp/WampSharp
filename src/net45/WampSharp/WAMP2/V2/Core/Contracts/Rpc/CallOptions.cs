using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Call)]
    public class CallOptions : WampDetailsOptions
    {
        [DataMember(Name = "timeout")]
        [PropertyName("timeout")]
        public int? TimeoutMili { get; set; }

        [DataMember(Name = "receive_progress")]
        [PropertyName("receive_progress")]
        public bool? ReceiveProgress { get; set; }

        [DataMember(Name = "disclose_me")]
        [PropertyName("disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}