using System;

namespace WampSharp.V2.PubSub
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