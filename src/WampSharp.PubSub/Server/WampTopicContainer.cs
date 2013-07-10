using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public class WampTopicContainer<TMessage> : IWampTopicContainer
    {
        #region Fields

        private readonly ConcurrentDictionary<string, WampTopic> mTopicUriToSubject;
        
        #endregion

        #region Constructor

        public WampTopicContainer()
        {
            mTopicUriToSubject =
                new ConcurrentDictionary<string, WampTopic>();
        }

        #endregion

        #region Public Methods

        public IEnumerable<string> TopicUris
        {
            get
            {
                return mTopicUriToSubject.Keys;
            }
        }

        public IEnumerable<IWampTopic> Topics
        {
            get
            {
                return mTopicUriToSubject.Values;
            }
        }

        public IWampTopic CreateTopicByUri(string topicUri, bool persistent)
        {
            WampTopic wampTopic = CreateWampTopic(topicUri, persistent);
            
            IDictionary<string, WampTopic> casted = mTopicUriToSubject;
            
            casted.Add(topicUri, wampTopic);

            RaiseTopicCreated(wampTopic);

            return wampTopic;
        }

        public IWampTopic GetOrCreateTopicByUri(string topicUri, bool persistent)
        {
            // Pretty ugly.
            bool created = false;

            WampTopic result =
                mTopicUriToSubject.GetOrAdd(topicUri,
                                            key =>
                                                {
                                                    WampTopic topic = CreateWampTopic(topicUri, persistent);
                                                    created = true;
                                                    return topic;
                                                });

            if (created)
            {
                RaiseTopicCreated(result);
            }

            return result;
        }

        public IWampTopic GetTopicByUri(string topicUri)
        {
            return mTopicUriToSubject[topicUri];
        }

        public bool TryRemoveTopicByUri(string topicUri, out IWampTopic topic)
        {
            WampTopic value;
            bool result = mTopicUriToSubject.TryRemove(topicUri, out value);
            topic = value;

            if (result)
            {
                RaiseTopicRemoved(topicUri);
            }

            return result;
        }

        #endregion

        #region Private Methods

        private WampTopic CreateWampTopic(string topicUri, bool persistent)
        {
            return new WampTopic(topicUri,
                                 new WampTopicContainerDisposable(this, topicUri),
                                 persistent);
        }

        #endregion

        #region Events

        public event EventHandler<WampTopicCreatedEventArgs> TopicCreated;

        public event EventHandler<WampTopicRemovedEventArgs> TopicRemoved;

        private void RaiseTopicCreated(IWampTopic wampTopic)
        {
            EventHandler<WampTopicCreatedEventArgs> topicCreated = TopicCreated;

            if (topicCreated != null)
            {
                topicCreated(this, new WampTopicCreatedEventArgs(wampTopic));
            }
        }

        private void RaiseTopicRemoved(string topicUri)
        {
            EventHandler<WampTopicRemovedEventArgs> topicRemoved = TopicRemoved;

            if (topicRemoved != null)
            {
                topicRemoved(this, new WampTopicRemovedEventArgs(topicUri));
            }
        }

        #endregion

        #region Nested Classes

        private class WampTopicContainerDisposable : IDisposable
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
                IWampTopic topic;
                mParent.TryRemoveTopicByUri(mTopicUri, out topic);
            }
        }

        #endregion
    }
}