using System;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for an event where a topic was created.
    /// </summary>
    public class WampTopicCreatedEventArgs : WampTopicEventArgs
    {
        public WampTopicCreatedEventArgs(IWampTopic topic) : base(topic)
        {
        }
    }
}