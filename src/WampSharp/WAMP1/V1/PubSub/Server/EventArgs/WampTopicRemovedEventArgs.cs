using System;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for <see cref="IWampTopic"/> destruction
    /// event.
    /// </summary>
    public class WampTopicRemovedEventArgs : WampTopicEventArgs
    {
        /// <summary>
        /// Creates a new instance of <see cref="WampTopicRemovedEventArgs"/>.
        /// </summary>
        /// <param name="topic">The removed topic.</param>
        public WampTopicRemovedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}