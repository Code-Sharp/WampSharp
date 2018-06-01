using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal abstract class MatchTopicContainer
    {
        #region Fields

        private readonly ConcurrentDictionary<string, IWampTopic> mTopicUriToSubject;
        private readonly object mLock = new object();
        private readonly WampIdMapper<IWampTopic> mSubscriptionIdToTopic;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampTopicContainer"/>.
        /// </summary>
        /// <param name="subscriptionIdToTopic"></param>
        public MatchTopicContainer(WampIdMapper<IWampTopic> subscriptionIdToTopic)
        {
            mTopicUriToSubject =
                new ConcurrentDictionary<string, IWampTopic>();

            mSubscriptionIdToTopic = subscriptionIdToTopic;
        }

        #endregion

        #region Public Methods

        public IEnumerable<string> TopicUris => mTopicUriToSubject.Keys;

        public IEnumerable<IWampTopic> Topics => mTopicUriToSubject.Values;

        public IWampRegistrationSubscriptionToken Subscribe(IWampRawTopicRouterSubscriber subscriber, string topicUri, SubscribeOptions options)
        {
            lock (mLock)
            {
                IWampTopic topic = GetOrCreateTopicByUri(topicUri);

                IDisposable disposable = topic.Subscribe(subscriber);

                var result = new SubscriptionToken(topic.SubscriptionId, disposable);

                return result;
            }
        }

        public bool Publish<TMessage>(IWampFormatter<TMessage> formatter,
                                      long publicationId,
                                      PublishOptions options,
                                      string topicUri)
        {
            return PublishSafe(topicUri, options, topic =>
                                   topic.Publish(formatter, publicationId, options));
        }

        public bool Publish<TMessage>(IWampFormatter<TMessage> formatter,
                                      long publicationId,
                                      PublishOptions options,
                                      string topicUri,
                                      TMessage[] arguments)
        {
            return PublishSafe(topicUri, options, topic =>
                                   topic.Publish(formatter, publicationId, options, arguments));
        }

        public bool Publish<TMessage>(IWampFormatter<TMessage> formatter,
                                      long publicationId,
                                      PublishOptions options,
                                      string topicUri,
                                      TMessage[] arguments,
                                      IDictionary<string, TMessage> argumentKeywords)
        {
            return PublishSafe(topicUri, options, topic =>
                                   topic.Publish(formatter, publicationId, options, arguments, argumentKeywords));
        }

        private bool PublishSafe
            (string topicUri, PublishOptions publishOptions, Action<IWampTopic> invoker)
        {
            lock (mLock)
            {
                bool anyTopics = false;

                IEnumerable<IWampTopic> topics = GetMatchingTopics(topicUri, publishOptions);

                foreach (IWampTopic topic in topics)
                {
                    // Some topics are persistent and therefore publishing to them always succeeds.
                    if (topic.HasSubscribers)
                    {
                        anyTopics = true;
                    }

                    invoker(topic);
                }

                return anyTopics;
            }
        }

        public IWampTopic CreateTopicByUri(string topicUri, bool persistent)
        {
            WampTopic wampTopic = CreateWampTopic(topicUri, persistent);

            IDictionary<string, IWampTopic> casted = mTopicUriToSubject;

            casted.Add(topicUri, wampTopic);

            RaiseTopicCreated(wampTopic);

            return wampTopic;
        }

        public IWampTopic GetOrCreateTopicByUri(string topicUri)
        {
            // Pretty ugly.
            bool created = false;

            IWampTopic result;


            result =
                mTopicUriToSubject.GetOrAdd(topicUri,
                                            key =>
                                            {
                                                IWampTopic topic = CreateWampTopic(topicUri, false);
                                                created = true;
                                                return topic;
                                            });

            if (created)
            {
                RaiseTopicCreated(result);
            }

            return result;
        }

        protected IWampTopic GetOrCreateRetainingTopicByUri(string topicUri)
        {
            lock (mLock)
            {
                IWampTopic topic = GetOrCreateTopicByUri(topicUri);

                if (!(topic is WampRetainingTopic))
                {
                    topic.TopicEmpty -= OnTopicEmpty;
                    topic = new WampRetainingTopic(topic);
                    mTopicUriToSubject[topicUri] = topic;
                }

                return topic;
            }
        }

        public IWampTopic GetTopicByUri(string topicUri)
        {

            if (mTopicUriToSubject.TryGetValue(topicUri, out IWampTopic result))
            {
                return result;
            }

            return null;
        }

        public bool TryRemoveTopicByUri(string topicUri, out IWampTopic topic)
        {
            bool result = mTopicUriToSubject.TryRemove(topicUri, out IWampTopic value);
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

            long subscriptionId = mSubscriptionIdToTopic.Add(topic);

            topic.SubscriptionId = subscriptionId;

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

                    mSubscriptionIdToTopic.TryRemoveExact(topic.SubscriptionId, topic);

                    if (mTopicUriToSubject.TryRemoveExact(topic.TopicUri, topic))
                    {
                        RaiseTopicRemoved(topic);
                    }
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler<WampTopicCreatedEventArgs> TopicCreated;

        public event EventHandler<WampTopicRemovedEventArgs> TopicRemoved;

        private void RaiseTopicCreated(IWampTopic wampTopic)
        {
            TopicCreated?.Invoke(this, new WampTopicCreatedEventArgs(wampTopic));
        }

        private void RaiseTopicRemoved(IWampTopic topic)
        {
            TopicRemoved?.Invoke(this, new WampTopicRemovedEventArgs(topic));
        }

        #endregion

        #region Abstract Methods

        public abstract IWampCustomizedSubscriptionId GetSubscriptionId(string topicUri, SubscribeOptions options);

        public abstract IEnumerable<IWampTopic> GetMatchingTopics(string criteria, PublishOptions publishOptions = null);

        public abstract bool Handles(SubscribeOptions options);

        #endregion
    }
}