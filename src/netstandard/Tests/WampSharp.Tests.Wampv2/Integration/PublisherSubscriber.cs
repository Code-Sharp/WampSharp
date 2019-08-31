using WampSharp.V2;

namespace WampSharp.Tests.Wampv2.Integration
{
    internal class PublisherSubscriber
    {
        public IWampChannel Publisher { get; set; }
        public IWampChannel Subscriber { get; set; }
        public long PublisherSessionId { get; set; }
    }
}