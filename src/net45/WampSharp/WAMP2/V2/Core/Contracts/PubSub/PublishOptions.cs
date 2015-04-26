using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class PublishOptions : WampDetailsOptions
    {
        public PublishOptions()
        {
        }

        protected PublishOptions(PublishOptions options)
        {
            this.Acknowledge = options.Acknowledge;
            this.ExcludeMe = options.ExcludeMe;
            this.Exclude = options.Exclude;
            this.Eligible = options.Eligible;
            this.DiscloseMe = options.DiscloseMe;
            this.OriginalValue = options.OriginalValue;
        }

        [DataMember(Name = "acknowledge")]
        public bool? Acknowledge { get; set; }

        [DataMember(Name = "exclude_me")]
        public bool? ExcludeMe { get; set; }

        [DataMember(Name = "exclude")]
        public long[] Exclude { get; set; }

        [DataMember(Name = "eligible")]
        public long[] Eligible { get; set; }

        [DataMember(Name = "disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}