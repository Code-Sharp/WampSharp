namespace WampSharp.V2.Client
{
    public interface IWampTopicContainerProxy
    {
        IWampTopicProxy GetTopicByUri(string topicUri);
    }
}