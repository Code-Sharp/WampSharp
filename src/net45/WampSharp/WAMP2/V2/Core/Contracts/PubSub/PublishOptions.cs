using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class PublishOptions : WampDetailsOptions
    {
        [PropertyName("acknowledge")]
        public bool? Acknowledge { get; set; }

        [PropertyName("exclude_me")]
        public bool? ExcludeMe { get; set; }

        [PropertyName("exclude")]
        public long[] Exclude { get; set; }

        [PropertyName("eligible")]
        public long[] Eligible { get; set; }

        [PropertyName("disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}