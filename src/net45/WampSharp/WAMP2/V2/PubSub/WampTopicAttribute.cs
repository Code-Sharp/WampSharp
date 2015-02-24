using System;

namespace WampSharp.V2.PubSub
{
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class WampTopicAttribute : Attribute
    {
        private readonly string mTopic;

        public WampTopicAttribute(string topic)
        {
            mTopic = topic;
        }

        public string Topic
        {
            get
            {
                return mTopic;
            }
        }
    }
}