namespace WampSharp.V2.PubSub
{
    public class PrefixSubscriptionId : SimpleSubscriptionId
    {
        public PrefixSubscriptionId(string topicUri) : base(topicUri)
        {
        }
    }
}