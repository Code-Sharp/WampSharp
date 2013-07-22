namespace WampSharp.PubSub.Server
{
    public class WampTopicRemovedEventArgs : WampTopicEventArgs
    {
        public WampTopicRemovedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}