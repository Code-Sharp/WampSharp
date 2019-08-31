using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.PubSub.Helpers
{
    public class WampPublishRequest<TMessage>
    {
        public IWampPubSubClient<TMessage> Client { get; set; }
        public string TopicUri { get; set; }
        public bool? ExcludeMe { get; set; }
        public string[] Exclude { get; set; }
        public string[] Eligible { get; set; }
        public object Event { get; set; }
    }
}