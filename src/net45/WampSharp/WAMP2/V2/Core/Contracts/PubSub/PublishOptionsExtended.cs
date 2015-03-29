namespace WampSharp.V2.Core.Contracts
{
    public class PublishOptionsExtended : PublishOptions
    {
        public PublishOptionsExtended(PublishOptions options) : base(options)
        {
        }

        [IgnoreProperty]
        public long PublisherId { get; set; }

        [IgnoreProperty]
        public string TopicUri { get; set; }
    }
}