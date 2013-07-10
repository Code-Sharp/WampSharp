namespace WampSharp.PubSub.Server
{
    public class WampTopicCreatedEventArgs : WampTopicEventArgs
    {
        private readonly IWampTopic mTopic;

        public WampTopicCreatedEventArgs(IWampTopic topic) : base(topic.TopicUri)
        {
            mTopic = topic;
        }

        public IWampTopic Topic
        {
            get
            {
                return mTopic;
            }
        }
    }
}