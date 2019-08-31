using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for <see cref="IWampTopic"/>
    /// creation/destruction events.
    /// </summary>
    public class WampTopicEventArgs : System.EventArgs
    {

        /// <summary>
        /// Creates a new instance of <see cref="WampTopicEventArgs"/>.
        /// </summary>
        /// <param name="topic">The relevant topic.</param>
        public WampTopicEventArgs(IWampTopic topic)
        {
            Topic = topic;
        }

        /// <summary>
        /// Gets the relevant topic.
        /// </summary>
        public IWampTopic Topic { get; }
    }
}