using System;

namespace WampSharp.PubSub.Server
{
    public class WampTopicEventArgs : EventArgs
    {
        private readonly string mTopicUri;

        public WampTopicEventArgs(string topicUri)
        {
            mTopicUri = topicUri;
        }

        public string TopicUri
        {
            get
            {
                return mTopicUri;
            }
        }
    }
}