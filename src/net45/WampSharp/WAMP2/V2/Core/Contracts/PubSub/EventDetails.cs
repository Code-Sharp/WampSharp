using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class EventDetails
    {
        [DataMember(Name = "publisher")]
        [PropertyName("publisher")]
        public long? Publisher { get; set; }
    }
}