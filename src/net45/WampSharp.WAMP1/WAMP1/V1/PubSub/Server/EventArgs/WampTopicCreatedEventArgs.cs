using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for an event that occurs when a new
    /// <see cref="IWampTopic"/> is created.
    /// </summary>
    public class WampTopicCreatedEventArgs : WampTopicEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="WampTopicCreatedEventArgs"/>.
        /// </summary>
        /// <param name="topic">The created topic.</param>
        public WampTopicCreatedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}