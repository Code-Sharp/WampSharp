namespace WampSharp.PubSub.Server
{
    public class WampTopicCreatedEventArgs : WampTopicEventArgs
    {
        public WampTopicCreatedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}