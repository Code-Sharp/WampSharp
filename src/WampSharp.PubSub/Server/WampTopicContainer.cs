using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public class WampTopicContainer<TMessage>
    {
        private readonly ConcurrentDictionary<string, WampTopic<TMessage>> mTopicUriToSubject;

        public WampTopicContainer()
        {
            mTopicUriToSubject =
                new ConcurrentDictionary<string, WampTopic<TMessage>>();
        }

        public IEnumerable<string> TopicUris
        {
            get
            {
                return mTopicUriToSubject.Keys;
            }
        }

        public WampTopic<TMessage> GetTopicByUri(string topicUri)
        {
            WampTopic<TMessage> result =
                mTopicUriToSubject.GetOrAdd(topicUri, 
                x => new WampTopic<TMessage>());

            return result;
        }

        public void RemoveTopicByUri(string topicUri)
        {
            WampTopic<TMessage> value;
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