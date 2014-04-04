namespace WampSharp.V2.Client
{
    public interface IWampTopicContainerProxy
    {
        IWampTopicProxy GetTopic(string topicUri);
    }
}