using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.PubSub.Server
{
    public class WampTopicContainer<TMessage> : IWampTopicContainerExtended<TMessage>
    {
        #region Fields

        private readonly ConcurrentDictionary<string, WampTopic> mTopicUriToSubject;
        private readonly object mLock = new object();

        #endregion

        #region Constructor

        public WampTopicContainer()
        {
            mTopicUriToSubject =
                new ConcurrentDictionary<string, WampTopic>();
        }

        #endregion

        #region Public Methods

        public IDisposable Subscribe(string topicUri, IObserver<object> observer)
        {
            lock (mLock)
            {
                IWampTopic topic = GetOrCreateTopicByUri(topicUri, false);

                return topic.Subscribe(observer);
            }
        }

        public void Unsubscribe(string topicUri, string sessionId)
        {
            lock (mLock)
            {
                IWampTopic topic = GetTopicByUri(topicUri);

                if (topic != null)
                {
                    topic.Unsubscribe(sessionId);                    
                }
            }
        }

        public void Publish(string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            lock (mLock)
            {
                IWampTopic topic = GetTopicByUri(topicUri);

                if (topic != null)
                {
                    topic.OnNext(new WampNotification(@event, exclude, eligible));                    
                }
            }
        }

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
            WampTopic result;

            if (mTopicUriToSubject.TryGetValue(topicUri, out result))
            {
                return result;
            }

            return null;
        }

        public bool TryRemoveTopicByUri(string topicUri, out IWampTopic topic)
        {
            WampTopic value;
            bool result = mTopicUriToSubject.TryRemove(topicUri, out value);
            topic = value;

            if (result)
            {
                RaiseTopicRemoved(topic);
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

        private void RaiseTopicRemoved(IWampTopic topic)
        {
            EventHandler<WampTopicRemovedEventArgs> topicRemoved = TopicRemoved;

            if (topicRemoved != null)
            {
                topicRemoved(this, new WampTopicRemovedEventArgs(topic));
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
                // Yuck
                lock (mParent.mLock)
                {
                    IWampTopic topic;
                    mParent.TryRemoveTopicByUri(mTopicUri, out topic);                    
                }
            }
        }

        #endregion
    }
}