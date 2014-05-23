using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class WampTopicContainer : IWampTopicContainer
    {
        #region Fields

        private readonly ConcurrentDictionary<string, WampTopic> mTopicUriToSubject;
        private readonly object mLock = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampTopicContainer"/>.
        /// </summary>
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

        public IDisposable Subscribe(IWampTopicSubscriber subscriber, object options, string topicUri)
        {
            lock (mLock)
            {
                IWampTopic topic = GetOrCreateTopicByUri(topicUri, false);

                return topic.Subscribe(subscriber, options);
            }
        }

        public long Publish(object options, string topicUri)
        {
            return TopicInvokeSafe(topicUri,
                                   topic => topic.Publish(options));
        }

        public long Publish(object options, string topicUri, object[] arguments)
        {
            return TopicInvokeSafe(topicUri,
                                   topic => topic.Publish(options, arguments));
        }

        public long Publish(object options, string topicUri, object[] arguments, object argumentKeywords)
        {
            return TopicInvokeSafe(topicUri,
                                   topic => topic.Publish(options, arguments, argumentKeywords));
        }

        private TResult TopicInvokeSafe<TResult>
            (string topicUri, Func<IWampTopic, TResult> invoker)
        {
            lock (mLock)
            {
                IWampTopic topic = GetTopicByUri(topicUri);

                if (topic != null)
                {
                    return invoker(topic);
                }
                else
                {
                    throw new WampException(WampErrors.InvalidTopic,
                                            "topicUri: " + topicUri);
                }
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
            WampTopic topic = new WampTopic(topicUri, persistent);

            // Non persistent topics die when they are empty :)
            if (!persistent)
            {
                topic.TopicEmpty += OnTopicEmpty;
            }

            return topic;
        }

        private void OnTopicEmpty(object sender, EventArgs e)
        {
            lock (mLock)
            {
                IWampTopic topic = sender as IWampTopic;

                if (!topic.HasSubscribers)
                {
                    topic.TopicEmpty -= OnTopicEmpty;
                    topic.Dispose();

                    IWampTopic deletedTopic;
                    TryRemoveTopicByUri(topic.TopicUri, out deletedTopic);
                }
            }
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
    }
}