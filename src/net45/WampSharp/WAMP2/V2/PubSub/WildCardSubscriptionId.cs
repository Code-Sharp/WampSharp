namespace WampSharp.V2.PubSub
{
    public class WildCardSubscriptionId : SimpleSubscriptionId
    {
        public WildCardSubscriptionId(string topicUri) : base(topicUri)
        {
        }
    }
}