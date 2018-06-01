using System.Collections.Generic;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.PubSub.Helpers
{
    public class MockWampPubSubRequestManager<TMessage>
    {
        private readonly ICollection<WampSubscribeRequest<TMessage>> mSubscriptionRemovals = new List<WampSubscribeRequest<TMessage>>();

        public ICollection<WampSubscribeRequest<TMessage>> SubscriptionRemovals => mSubscriptionRemovals;

        public ICollection<WampSubscribeRequest<TMessage>> Subscriptions { get; } = new List<WampSubscribeRequest<TMessage>>();

        public ICollection<WampPublishRequest<TMessage>> Publications { get; } = new List<WampPublishRequest<TMessage>>();

        public IWampServer GetServer(IWampPubSubClient<TMessage> client)
        {
            return new MockWampPubSubServerProxy(this, client);
        }

        private class MockWampPubSubServerProxy : IWampServer
        {
            private readonly MockWampPubSubRequestManager<TMessage> mParent;
            private readonly IWampPubSubClient<TMessage> mClient;

            public MockWampPubSubServerProxy(MockWampPubSubRequestManager<TMessage> parent, IWampPubSubClient<TMessage> client)
            {
                mParent = parent;
                mClient = client;
            }

            public void Prefix(IWampClient client, string prefix, string uri)
            {
            }

            public void Call(IWampClient client, string callId, string procUri, params object[] arguments)
            {
            }

            public void Subscribe(IWampClient client, string topicUri)
            {
                mParent.Subscriptions.Add(new WampSubscribeRequest<TMessage>()
                {
                    Client = mClient,
                    TopicUri = topicUri
                });
            }

            public void Unsubscribe(IWampClient client, string topicUri)
            {
                mParent.mSubscriptionRemovals.Add(new WampSubscribeRequest<TMessage>()
                {
                    Client = mClient,
                    TopicUri = topicUri
                });
            }

            public void Publish(IWampClient client, string topicUri, object @event)
            {
                InnerPublish(client, topicUri, @event);
            }

            public void Publish(IWampClient client, string topicUri, object @event, bool excludeMe)
            {
                InnerPublish(client, topicUri, @event, excludeMe);
            }

            public void Publish(IWampClient client, string topicUri, object @event, string[] exclude)
            {
                InnerPublish(client, topicUri, @event, null, exclude);
            }

            public void Publish(IWampClient client, string topicUri, object @event, string[] exclude, string[] eligible)
            {
                InnerPublish(client, topicUri, @event, null, exclude, eligible);
            }

            private void InnerPublish(IWampClient client, string topicUri, object @event, bool? excludeMe = null, string[] exclude = null, string[] eligible = null)
            {
                mParent.Publications.Add(new WampPublishRequest<TMessage>()
                {
                    Client = mClient,
                    Eligible = eligible,
                    Event = @event,
                    ExcludeMe = excludeMe,
                    Exclude = exclude,
                    TopicUri = topicUri
                });
            }

        }
    }
}