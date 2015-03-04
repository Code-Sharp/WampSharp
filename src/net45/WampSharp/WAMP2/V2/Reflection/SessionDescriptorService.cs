using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Reflection
{
    public class SubscriptionDescriptorService : IWampSubscriptionDescriptor
    {
        private readonly IWampHostedRealm mRealm;
        private readonly SubscriptionMetadataSubscriber mSubscriber;
        private readonly ConcurrentDictionary<long, ConcurrentBag<long>> mSubscriptionIdToSubscribers =
            new ConcurrentDictionary<long, ConcurrentBag<long>>();

        public SubscriptionDescriptorService(IWampHostedRealm realm)
        {
            mRealm = realm;
            mSubscriber = new SubscriptionMetadataSubscriber(realm.TopicContainer);

            mRealm.TopicContainer.TopicCreated += OnTopicCreated;
            mRealm.TopicContainer.TopicRemoved += OnTopicRemoved;
        }

        private void OnTopicCreated(object sender, WampTopicCreatedEventArgs e)
        {
            e.Topic.SubscriptionAdded += OnSubscriptionAdded;
            e.Topic.SubscriptionRemoved += OnSubscriptionRemoved;
        }

        private void OnSubscriptionAdded(object sender, WampSubscriptionAddEventArgs e)
        {
            IRemoteWampTopicSubscriber subscriber = e.Subscriber;

            mSubscriber.OnSubscribe(subscriber.SessionId, subscriber.SubscriptionId);
        }

        private void OnSubscriptionRemoved(object sender, WampSubscriptionRemoveEventArgs e)
        {
            mSubscriber.OnUnsubscribe(e.Session, e.SubscriptionId);
        }

        private void OnTopicRemoved(object sender, WampTopicRemovedEventArgs e)
        {
            e.Topic.SubscriptionAdded -= OnSubscriptionAdded;
            e.Topic.SubscriptionRemoved -= OnSubscriptionRemoved;
        }


        public AvailableSubscriptions GetAllSubscriptionIds()
        {
            throw new System.NotImplementedException();
        }

        public long LookupSubscriptionId(string topicUri, SubscribeOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public long[] GetMatchingSubscriptionIds(string topicUri)
        {
            throw new System.NotImplementedException();
        }

        public SubscriptionDetails GetSubscriptionDetails(long subscriptionId)
        {
            throw new System.NotImplementedException();
        }

        public long[] GetSubscribers(long subscriptionId)
        {
            throw new System.NotImplementedException();
        }

        public long CountSubscribers(long subscriptionId)
        {
            throw new System.NotImplementedException();
        }


        private class SubscriptionMetadataSubscriber : IWampSubscriptionMetadataSubscriber
        {
            private readonly IWampTopic mOnCreateTopic;
            private readonly IWampTopic mOnSubscribeTopic;
            private readonly IWampTopic mOnDeleteTopic;
            private readonly IWampTopic mOnUnsubscribeTopic;
            private readonly PublishOptions mPublishOptions = new PublishOptions();

            public SubscriptionMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mOnCreateTopic = topicContainer.CreateTopicByUri("wamp.subscription.on_create", true);
                mOnSubscribeTopic = topicContainer.CreateTopicByUri("wamp.subscription.on_subscribe", true);
                mOnUnsubscribeTopic = topicContainer.CreateTopicByUri("wamp.subscription.on_unsubscribe", true);
                mOnDeleteTopic = topicContainer.CreateTopicByUri("wamp.subscription.on_delete", true);
            }

            public void OnCreate(long sessionId, SubscriptionDetails details)
            {
                mOnCreateTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] { sessionId, details });
            }

            public void OnSubscribe(long sessionId, long subscriptionId)
            {
                mOnCreateTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] { sessionId, subscriptionId });
            }

            public void OnUnsubscribe(long sessionId, long subscriptionId)
            {
                mOnCreateTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] { sessionId, subscriptionId });
            }

            public void OnDelete(long sessionId, long subscriptionId)
            {
                mOnCreateTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] { sessionId, subscriptionId });
            }
        }
    }

    public class SessionDescriptorService : IWampSessionDescriptor
    {
        private readonly IWampHostedRealm mRealm;
        private readonly IWampSessionMetadataSubscriber mSubscriber;

        private readonly ConcurrentDictionary<long, WampSessionDetails> mSessionIdToDetails =
            new ConcurrentDictionary<long, WampSessionDetails>(); 

        public SessionDescriptorService(IWampHostedRealm realm)
        {
            mRealm = realm;
            mSubscriber = new SessionMetadataSubscriber(realm.TopicContainer);

            mRealm.SessionClosed += OnSessionClosed;
            mRealm.SessionCreated += OnSessionCreated;
        }

        private void OnSessionCreated(object sender, WampSessionEventArgs e)
        {
            WampSessionDetails sessionDetails = new WampSessionDetails()
            {
                Realm = mRealm.Name,
                Session = e.SessionId,
                AuthMethod = "anonymous"
            };
            
            mSessionIdToDetails[e.SessionId] = sessionDetails;

            mSubscriber.OnJoin(sessionDetails);
        }

        private void OnSessionClosed(object sender, WampSessionCloseEventArgs e)
        {
            mSubscriber.OnLeave(e.SessionId);
        }

        public long SessionCount()
        {
            return mSessionIdToDetails.Count;
        }

        public long[] GetSessionIds()
        {
            return mSessionIdToDetails.Keys.ToArray();
        }

        public WampSessionDetails GetSessionDetails(long sessionId)
        {
            WampSessionDetails result;

            if (mSessionIdToDetails.TryGetValue(sessionId, out result))
            {
                return result;
            }

            return null;
        }

        private class SessionMetadataSubscriber : IWampSessionMetadataSubscriber
        {
            private readonly IWampTopic mOnJoinTopic;
            private readonly IWampTopic mOnLeaveTopic;
            private readonly PublishOptions mPublishOptions = new PublishOptions();

            public SessionMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mOnJoinTopic = topicContainer.CreateTopicByUri("wamp.session.on_join", true);
                mOnLeaveTopic = topicContainer.CreateTopicByUri("wamp.session.on_leave", true);
            }

            public void OnJoin(WampSessionDetails details)
            {
                mOnJoinTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] {details});
            }

            public void OnLeave(long sessionId)
            {
                mOnLeaveTopic.Publish(WampObjectFormatter.Value, mPublishOptions, new object[] { sessionId });
            }
        }
    }
}