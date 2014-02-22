namespace WampSharp.V2.PubSub
{
    public interface IWampTopicSubscriber
    {
        void Event(long publicationId, object details);
        void Event(long publicationId, object details, object[] arguments);
        void Event(long publicationId, object details, object[] arguments, object argumentsKeywords);
    }
}