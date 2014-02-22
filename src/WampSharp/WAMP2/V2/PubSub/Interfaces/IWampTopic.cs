namespace WampSharp.V2.PubSub
{
    public interface IWampTopic
    {
        long Publish(object options, string topic);
        long Publish(object options, string topic, object[] arguments);
        long Publish(object options, string topic, object[] arguments, object argumentKeywords);
        long Subscribe(IWampTopicSubscriber subscriber, object options, string topicUri);
        void Unsubscribe(IWampTopicSubscriber subscriber, long subscriptionId);
    }
}