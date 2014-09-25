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

        /// <summary>
        /// Gets the relevant topic.
        /// </summary>
        public IWampTopic Topic
        {
            get
            {
                return mTopic;
            }
        }
    }
}