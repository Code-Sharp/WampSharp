using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.MetaApi
{
    public class HistoricalSubscription
    {
        public string TopicUri { get; set; }

        public SubscribeOptions SubscribeOptions { get; set; }

        public long SubscriptionId { get; internal set; }
    }
}