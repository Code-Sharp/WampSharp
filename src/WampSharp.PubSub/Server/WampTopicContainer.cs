using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public class WampTopicContainer<TMessage>
    {
        private readonly ConcurrentDictionary<string, WampTopic> mTopicUriToSubject;

        public WampTopicContainer()
        {
            mTopicUriToSubject =
                new ConcurrentDictionary<string, WampTopic>();
        }

        public IEnumerable<string> TopicUris
        {
            get
            {
                return mTopicUriToSubject.Keys;
            }
        }

        public WampTopic GetTopicByUri(string topicUri)
        {
            WampTopic result =
                mTopicUriToSubject.GetOrAdd(topicUri, 
                x => new WampTopic());

            return result;
        }

        public void RemoveTopicByUri(string topicUri)
        {
            WampTopic value;
            mTopicUriToSubject.TryRemove(topicUri, out value);
        }

        public class WampTopicContainerDisposable : IDisposable
        {
            private readonly WampTopicContainer<TMessage> mParent;
            private readonly string mTopicUri;

            public WampTopicContainerDisposable(WampTopicContainer<TMessage> parent, string topicUri)
            {
                mParent = parent;
                mTopicUri = topicUri;
            }

            public void Dispose()
            {
                mParent.RemoveTopicByUri(mTopicUri);
            }
        }
    }
}