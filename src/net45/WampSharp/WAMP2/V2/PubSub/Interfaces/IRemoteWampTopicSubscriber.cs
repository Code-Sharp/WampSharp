namespace WampSharp.V2.PubSub
{
    public interface IRemoteWampTopicSubscriber
    {
        void Event(object details);
        void Event(object details, object[] arguments);
        void Event(object details, object[] arguments, object argumentsKeywords);         
    }
}