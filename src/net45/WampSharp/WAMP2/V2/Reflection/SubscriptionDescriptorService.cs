using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Reflection
{
    public class SubscriptionDescriptorService : IWampSubscriptionDescriptor
    {
        private readonly IWampHostedRealm mRealm;
        private readonly SubscriptionMetadataSubscriber mSubscriber;

        private ImmutableDictionary<string, ImmutableList<SubscriptionDetailsExtended>> mTopicUriToSubscriptions =
            ImmutableDictionary<string, ImmutableList<SubscriptionDetailsExtended>>.Empty;

        private ImmutableDictionary<long, SubscriptionDetailsExtended> mSubscriptionIdToDetails =
            ImmutableDictionary<long, SubscriptionDetailsExtended>.Empty;

        private readonly object mLock = new object();

        public SubscriptionDescriptorService(IWampHostedRealm realm)
        {
            mRealm = realm;

            IWampTopicContainer topicContainer = realm.TopicContainer;

            mSubscriber = new SubscriptionMetadataSubscriber(topicContainer);

            IObservable<IWampTopic> removed = GetTopicRemoved(topicContainer);

            var observable =
                from topic in GetTopicCreated(topicContainer)
                let topicRemoved = removed.Where(x => x == topic)
                let subscriptionAdded = GetSubscriptionAdded(topic, topicRemoved)
                let subscriptionRemoved = GetSubscriptionRemoved(topic, topicRemoved)
                select new {topic, subscriptionAdded, subscriptionRemoved};

            var addObservable =
                from item in observable
                from eventArgs in item.subscriptionAdded
                select new {Topic = item.topic, EventArgs = eventArgs};

            var removeObservable =
                from item in observable
                from eventArgs in item.subscriptionRemoved
                select new {Topic = item.topic, EventArgs = eventArgs};

            addObservable.Subscribe(x => OnSubscriptionAdded(x.Topic, x.EventArgs));
            removeObservable.Subscribe(x => OnSubscriptionRemoved(x.Topic, x.EventArgs));
        }

        private static IObservable<WampSubscriptionAddEventArgs> GetSubscriptionAdded(IWampTopic topic, IObservable<IWampTopic> removed)
        {
            return GetSubscriptionAdded(topic)
                .TakeUntil(removed);
        }

        private static IObservable<WampSubscriptionRemoveEventArgs> GetSubscriptionRemoved(IWampTopic topic, IObservable<IWampTopic> removed)
        {
            return GetSubscriptionRemoved(topic)
                .TakeUntil(removed);
        }

        private static IObservable<IWampTopic> GetTopicRemoved(IWampTopicContainer topicContainer)
        {
            return Observable.FromEventPattern<WampTopicRemovedEventArgs>
                (x => topicContainer.TopicRemoved += x,
                 x => topicContainer.TopicRemoved -= x)
                             .Select(x => x.EventArgs.Topic);
        }

        private static IObservable<IWampTopic> GetTopicCreated(IWampTopicContainer topicContainer)
        {
            return Observable.FromEventPattern<WampTopicCreatedEventArgs>
                (x => topicContainer.TopicCreated += x,
                 x => topicContainer.TopicCreated -= x)
                             .Select(x => x.EventArgs.Topic);
        }

        private static IObservable<WampSubscriptionAddEventArgs> GetSubscriptionAdded(IWampTopic topic)
        {
            return Observable.FromEventPattern<WampSubscriptionAddEventArgs>
                (x => topic.SubscriptionAdded += x,
                 x => topic.SubscriptionAdded -= x)
                             .Select(x => x.EventArgs);
        }

        private static IObservable<WampSubscriptionRemoveEventArgs> GetSubscriptionRemoved(IWampTopic topic)
        {
            return Observable.FromEventPattern<WampSubscriptionRemoveEventArgs>
                (x => topic.SubscriptionRemoved += x,
                 x => topic.SubscriptionRemoved -= x)
                             .Select(x => x.EventArgs);
        }


        private void OnSubscriptionAdded(IWampTopic topic, WampSubscriptionAddEventArgs e)
        {
            IRemoteWampTopicSubscriber subscriber = e.Subscriber;

            long sessionId = subscriber.SessionId;

            long subscriptionId = subscriber.SubscriptionId;

            lock (mLock)
            {
                SubscriptionDetailsExtended detailsExtended =
                    ImmutableInterlocked.GetOrAdd
                        (ref mSubscriptionIdToDetails,
                         subscriptionId,
                         x => GetSubscriptionDetails(topic,
                                                     sessionId,
                                                     subscriptionId,
                                                     e.Options));

                detailsExtended.AddSubscriber(sessionId);
            }

            mSubscriber.OnSubscribe(sessionId, subscriptionId);
        }

        private SubscriptionDetailsExtended GetSubscriptionDetails
            (IWampTopic topic,
             long sessionId,
             long subscriptionId,
             SubscribeOptions subscribeOptions)
        {
            SubscriptionDetailsExtended result =
                new SubscriptionDetailsExtended()
                {
                    SubscriptionId = subscriptionId,
                    Created = DateTime.Now,
                    Match = subscribeOptions.Match,
                    Uri = topic.TopicUri
                };

            mSubscriber.OnCreate(sessionId, result);

            var subscriptions =
                ImmutableInterlocked.GetOrAdd(ref mTopicUriToSubscriptions,
                                              topic.TopicUri,
                                              x => ImmutableList<SubscriptionDetailsExtended>.Empty);

            mTopicUriToSubscriptions =
                mTopicUriToSubscriptions.SetItem(topic.TopicUri, subscriptions.Add(result));

            return result;
        }

        private void OnSubscriptionRemoved(IWampTopic topic, WampSubscriptionRemoveEventArgs e)
        {
            SubscriptionDetailsExtended details;

            if (mSubscriptionIdToDetails.TryGetValue(e.SubscriptionId, out details))
            {
                details.RemoveSubscriber(e.Session);

                mSubscriber.OnUnsubscribe(e.Session, e.SubscriptionId);

                if (details.Subscribers.Count == 0)
                {
                    lock (mLock)
                    {
                        if (details.Subscribers.Count == 0)
                        {
                            DeleteSubscription(topic, e.Session, e.SubscriptionId, details);
                        }
                    }
                }
            }
        }

        private void DeleteSubscription(IWampTopic topic,
                                        long sessionId,
                                        long subscriptionId,
                                        SubscriptionDetailsExtended details)
        {
            mSubscriptionIdToDetails =
                mSubscriptionIdToDetails.Remove(subscriptionId);

            mSubscriber.OnDelete(sessionId, subscriptionId);

            DeleteTopicUriToSubscription(topic, details);
        }

        private void DeleteTopicUriToSubscription(IWampTopic topic, SubscriptionDetailsExtended details)
        {
            ImmutableList<SubscriptionDetailsExtended> subscriptions;

            if (mTopicUriToSubscriptions.TryGetValue(topic.TopicUri, out subscriptions))
            {
                subscriptions = subscriptions.Remove(details);

                if (subscriptions.Count != 0)
                {
                    mTopicUriToSubscriptions =
                        mTopicUriToSubscriptions.SetItem(topic.TopicUri, subscriptions);
                }
                else
                {
                    mTopicUriToSubscriptions =
                        mTopicUriToSubscriptions.Remove(topic.TopicUri);
                }
            }
        }

        public AvailableSubscriptions GetAllSubscriptionIds()
        {
            Dictionary<string, long[]> matchToSubscriptionId =
                mSubscriptionIdToDetails.Values.GroupBy(x => x.Match, x => x.SubscriptionId)
                                        .ToDictionary(x => x.Key, x => x.ToArray());

            AvailableSubscriptions result = new AvailableSubscriptions();

            long[] subscriptions;

            // Yuck!
            if (matchToSubscriptionId.TryGetValue(WampMatchPattern.Exact, out subscriptions))
            {
                result.Exact = subscriptions;
            }
            if (matchToSubscriptionId.TryGetValue(WampMatchPattern.Prefix, out subscriptions))
            {
                result.Prefix = subscriptions;
            }
            if (matchToSubscriptionId.TryGetValue(WampMatchPattern.Wildcard, out subscriptions))
            {
                result.Wildcard = subscriptions;
            }

            return result;
        }

        public long LookupSubscriptionId(string topicUri, SubscribeOptions options = null)
        {
            string match = WampMatchPattern.Default;

            if (options != null)
            {
                match = options.Match ?? match;
            }

            ImmutableList<SubscriptionDetailsExtended> subscriptions;

            SubscriptionDetailsExtended result = null;

            if (mTopicUriToSubscriptions.TryGetValue(topicUri, out subscriptions))
            {
                result = subscriptions.FirstOrDefault(x => x.Match == match);
            }

            if (result != null)
            {
                return result.SubscriptionId;
            }

            throw new WampException(WampErrors.NoSuchSubscription);
        }

        public long[] GetMatchingSubscriptionIds(string topicUri)
        {
            ImmutableList<SubscriptionDetailsExtended> subscriptions;

            if (mTopicUriToSubscriptions.TryGetValue(topicUri, out subscriptions))
            {
                long[] result = subscriptions.Select(x => x.SubscriptionId).ToArray();

                return result;
            }

            throw new WampException(WampErrors.NoSuchSubscription);
        }

        public SubscriptionDetails GetSubscriptionDetails(long subscriptionId)
        {
            SubscriptionDetailsExtended details;

            if (mSubscriptionIdToDetails.TryGetValue(subscriptionId, out details))
            {
                return details;
            }

            throw new WampException(WampErrors.NoSuchSubscription);
        }

        public long[] GetSubscribers(long subscriptionId)
        {
            SubscriptionDetailsExtended details;

            if (mSubscriptionIdToDetails.TryGetValue(subscriptionId, out details))
            {
                return details.Subscribers.ToArray();
            }

            throw new WampException(WampErrors.NoSuchSubscription);
        }

        public long CountSubscribers(long subscriptionId)
        {
            SubscriptionDetailsExtended details;

            if (mSubscriptionIdToDetails.TryGetValue(subscriptionId, out details))
            {
                return details.Subscribers.Count;
            }

            throw new WampException(WampErrors.NoSuchSubscription);
        }

        private class SubscriptionMetadataSubscriber : IWampSubscriptionMetadataSubscriber
        {
            private readonly IWampTopicContainer mTopicContainer;
            private readonly PublishOptions mPublishOptions = new PublishOptions();
            private const string mOnCreateTopicUri = "wamp.subscription.on_create";
            private const string mOnSubscribeTopicUri = "wamp.subscription.on_subscribe";
            private const string mOnUnsubscribeTopicUri = "wamp.subscription.on_unsubscribe";
            private const string mOnDeleteTopicUri = "wamp.subscription.on_delete";

            public SubscriptionMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mTopicContainer = topicContainer;
                mTopicContainer.CreateTopicByUri(mOnCreateTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnSubscribeTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnUnsubscribeTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnDeleteTopicUri, true);
            }

            public void OnCreate(long sessionId, SubscriptionDetails details)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, details);
            }

            public void OnSubscribe(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnSubscribeTopicUri, sessionId, subscriptionId);
            }

            public void OnUnsubscribe(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnUnsubscribeTopicUri, sessionId, subscriptionId);
            }

            public void OnDelete(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnDeleteTopicUri, sessionId, subscriptionId);
            }
        }
    }
}