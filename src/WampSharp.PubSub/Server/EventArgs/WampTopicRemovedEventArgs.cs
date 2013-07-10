namespace WampSharp.PubSub.Server
{
    public class WampTopicRemovedEventArgs : WampTopicEventArgs
    {
        public WampTopicRemovedEventArgs(string topicUri) : base(topicUri)
        {
        }
    }
}