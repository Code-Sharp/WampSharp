using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Invocation)]
    public class InvocationDetails
    {
        [DataMember(Name = "receive_progress")]
        [PropertyName("receive_progress")]
        public bool? ReceiveProgress { get; set; }
    }
}