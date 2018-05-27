using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class WampRetainingTopic : IWampTopic
    {
        private readonly IWampTopic mTopic;
        private readonly IDisposable mDisposable;

        public WampRetainingTopic(IWampTopic topic)
        {
            mTopic = topic;
            mDisposable = mTopic.Subscribe(new RetentionSubscriber(mTopic));
        }

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding
        {
            add => mTopic.SubscriptionAdding += value;
            remove => mTopic.SubscriptionAdding -= value;
        }

        public event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded
        {
            add => mTopic.SubscriptionAdded += value;
            remove => mTopic.SubscriptionAdded -= value;
        }

        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving
        {
            add => mTopic.SubscriptionRemoving += value;
            remove => mTopic.SubscriptionRemoving -= value;
        }

        public event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved
        {
            add => mTopic.SubscriptionRemoved += value;
            remove => mTopic.SubscriptionRemoved -= value;
        }

        public event EventHandler TopicEmpty
        {
            add => mTopic.TopicEmpty += value;
            remove => mTopic.TopicEmpty -= value;
        }

        public void Dispose()
        {
            mTopic.Dispose();
            mDisposable.Dispose();
        }

        public bool HasSubscribers => mTopic.HasSubscribers;

        public string TopicUri => mTopic.TopicUri;

        public void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions publishOptions)
        {
            mTopic.Publish(formatter, publicationId, publishOptions);
        }

        public void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions publishOptions,
                                      TMessage[] arguments)
        {
            mTopic.Publish(formatter, publicationId, publishOptions, arguments);
        }

        public void Publish<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, PublishOptions publishOptions,
                                      TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords)
        {
            mTopic.Publish(formatter, publicationId, publishOptions, arguments, argumentKeywords);
        }

        public IDisposable Subscribe(IWampRawTopicRouterSubscriber subscriber)
        {
            return mTopic.Subscribe(subscriber);
        }

        public long SubscriptionId => mTopic.SubscriptionId;
    }
}