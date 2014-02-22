namespace WampSharp.V2.PubSub
{
    public interface IWampTopicContainer
    {
        void Subscribe(IWampTopicSubscriber subscriber, object options, string topicUri);
        void Unsubscribe(IWampTopicSubscriber subscriber);

        long Publish(object options, string topicUri);
        long Publish(object options, string topicUri, object[] arguments);
        long Publish(object options, string topicUri, object[] arguments, object argumentKeywords);         
    }
}