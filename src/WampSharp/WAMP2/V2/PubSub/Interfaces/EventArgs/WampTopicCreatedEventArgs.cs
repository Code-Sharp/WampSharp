namespace WampSharp.V2.PubSub
{
    public class WampTopicCreatedEventArgs : WampTopicEventArgs
    {
        public WampTopicCreatedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}