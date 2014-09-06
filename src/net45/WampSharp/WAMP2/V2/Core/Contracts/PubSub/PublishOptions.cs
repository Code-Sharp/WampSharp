using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class PublishOptions
    {
        [DataMember(Name = "acknowledge")]
        [PropertyName("acknowledge")]
        public bool? Acknowledge { get; set; }

        [DataMember(Name = "exclude_me")]
        [PropertyName("exclude_me")]
        public bool? ExcludeMe { get; set; }

        [DataMember(Name = "exclude")]
        [PropertyName("exclude")]
        public long[] Exclude { get; set; }

        [DataMember(Name = "eligible")]
        [PropertyName("eligible")]
        public long[] Eligible { get; set; }

        [DataMember(Name = "disclose_me")]
        [PropertyName("disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}