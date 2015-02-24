using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
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