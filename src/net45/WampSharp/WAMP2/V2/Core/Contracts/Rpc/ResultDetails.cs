using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Result)]
    public class ResultDetails : WampDetailsOptions
    {
        [DataMember(Name = "progress")]
        [PropertyName("progress")]
        public bool? Progress { get; set; }
    }
}