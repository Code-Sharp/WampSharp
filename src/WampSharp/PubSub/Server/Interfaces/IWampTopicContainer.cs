using System;
using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public interface IWampTopicContainer
    {
        IWampTopic CreateTopicByUri(string topicUri, bool persistent);
        IWampTopic GetOrCreateTopicByUri(string topicUri, bool persistent);
        IWampTopic GetTopicByUri(string topicUri);
        bool TryRemoveTopicByUri(string topicUri, out IWampTopic topic);

        IEnumerable<string> TopicUris { get; }
        IEnumerable<IWampTopic> Topics { get; } 

        event EventHandler<WampTopicCreatedEventArgs> TopicCreated;
        event EventHandler<WampTopicRemovedEventArgs> TopicRemoved;
    }
}