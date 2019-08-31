using System;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for an event where a topic was removed.
    /// </summary>
    public class WampTopicRemovedEventArgs : WampTopicEventArgs
    {
        public WampTopicRemovedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}