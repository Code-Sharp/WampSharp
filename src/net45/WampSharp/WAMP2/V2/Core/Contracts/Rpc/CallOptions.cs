using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Call)]
    public class CallOptions : WampDetailsOptions
    {
        public CallOptions()
        {
        }

        public CallOptions(CallOptions other)
        {
            TimeoutMili = other.TimeoutMili;
            ReceiveProgress = other.ReceiveProgress;
            DiscloseMe = other.DiscloseMe;
        }

        [IgnoreDataMember]
        [DataMember(Name = "timeout")]
        internal int? TimeoutMili { get; set; }

        [DataMember(Name = "receive_progress")]
        public bool? ReceiveProgress { get; set; }

        [DataMember(Name = "disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}