namespace WampSharp.V2.PubSub
{
    public class WampTopicRemovedEventArgs : WampTopicEventArgs
    {
        public WampTopicRemovedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}