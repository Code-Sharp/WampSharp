using System.Collections.Concurrent;
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
            private readonly IWampTopicContainer mTopicContainer;
            private readonly PublishOptions mPublishOptions = new PublishOptions();
            private const string mOnCreateTopicUri = "wamp.subscription.on_create";
            private const string mOnSubscribeTopicUri = "wamp.subscription.on_subscribe";
            private const string mOnUnsubscribeTopicUri = "wamp.subscription.on_unsubscribe";
            private const string mOnDeleteTopicUri = "wamp.subscription.on_delete";

            public SubscriptionMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mTopicContainer = topicContainer;
            }

            public void OnCreate(long sessionId, SubscriptionDetails details)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, details);
            }

            public void OnSubscribe(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, subscriptionId);
            }

            public void OnUnsubscribe(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, subscriptionId);
            }

            public void OnDelete(long sessionId, long subscriptionId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, subscriptionId);
            }
        }
    }
}