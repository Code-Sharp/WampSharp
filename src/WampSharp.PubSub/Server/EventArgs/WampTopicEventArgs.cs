using System;

namespace WampSharp.PubSub.Server
{
    public class WampTopicEventArgs : EventArgs
    {
        private readonly IWampTopic mTopic;

        public WampTopicEventArgs(IWampTopic topic)
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