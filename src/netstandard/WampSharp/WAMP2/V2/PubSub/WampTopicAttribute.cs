using System;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Indicates this event/method represents a WAMPv2 pub/sub topic.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method)]
    public sealed class WampTopicAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of <see cref="WampTopicAttribute"/> given
        /// the topic uri this event/method is mapped to.
        /// </summary>
        /// <param name="topic">The given the topic uri this method/event is mapped to.</param>
        public WampTopicAttribute(string topic)
        {
            Topic = topic;
        }

        /// <summary>
        /// Gets the topic uri this method/event is mapped to.
        /// </summary>
        public string Topic { get; }
    }
}