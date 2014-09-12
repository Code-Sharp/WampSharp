using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Subscribe)]
    public class SubscribeOptions : WampDetailsOptions
    {
        [DataMember(Name = "match")]
        [PropertyName("match")]
        public string Match { get; set; }
    }
}