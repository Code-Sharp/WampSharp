using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    internal class WampRawTopicContainer<TMessage> : IWampRawTopicContainer<TMessage>
    {
        private readonly IWampTopicContainer mTopicContainer;
        private readonly IWampEventSerializer<TMessage> mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly object mLock = new object();

        private readonly WampIdMapper<RawWampTopic<TMessage>> mSubscriptionIdToTopic =
            new WampIdMapper<RawWampTopic<TMessage>>();

        private readonly ConcurrentDictionary<string, RawWampTopic<TMessage>> mTopicUriToTopic =
            new ConcurrentDictionary<string, RawWampTopic<TMessage>>();

        public WampRawTopicContainer(IWampTopicContainer topicContainer,
                                     IWampEventSerializer<TMessage> eventSerializer,
                                     IWampBinding<TMessage> binding)
        {
            mTopicContainer = topicContainer;
            mEventSerializer = eventSerializer;
            mBinding = binding;
        }

        public long Subscribe(ISubscribeRequest<TMessage> request, SubscribeOptions options, string topicUri)
        {
            lock (mLock)
            {
                RawWampTopic<TMessage> rawTopic;

                if (!mTopicUriToTopic.TryGetValue(topicUri, out rawTopic))
                {
                    rawTopic = CreateRawTopic(topicUri);

                    IDisposable disposable =
                        mTopicContainer.Subscribe(rawTopic, topicUri, options);

                    rawTopic.SubscriptionDisposable = disposable;
                }

                rawTopic.Subscribe(request, options);
                
                return rawTopic.SubscriptionId;
            }
        }

        public void Unsubscribe(IUnsubscribeRequest<TMessage> request, long subscriptionId)
        {
            lock (mLock)
            {
                RawWampTopic<TMessage> rawTopic;

                if (!mSubscriptionIdToTopic.TryGetValue(subscriptionId, out rawTopic))
                {
                    throw new WampException(WampErrors.NoSuchSubscription, "subscriptionId: " + subscriptionId);
                }

                rawTopic.Unsubscribe(request);
            }
        }

        public long Publish(PublishOptions options, string topicUri)
        {
            return mTopicContainer.Publish(mBinding.Formatter, options, topicUri);
        }

        public long Publish(PublishOptions options, string topicUri, TMessage[] arguments)
        {
            return mTopicContainer.Publish(mBinding.Formatter, options, topicUri, arguments);
        }

        public long Publish(PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords)
        {
            return mTopicContainer.Publish(mBinding.Formatter, options, topicUri, arguments, argumentKeywords);
        }

        private void OnTopicEmpty(object sender, EventArgs e)
        {
            RawWampTopic<TMessage> rawTopic = sender as RawWampTopic<TMessage>;

            if (rawTopic != null)
            {
                lock (mLock)
                {
                    if (!rawTopic.HasSubscribers)
                    {
                        mSubscriptionIdToTopic.TryRemove(rawTopic.SubscriptionId, out rawTopic);
                        mTopicUriToTopic.TryRemove(rawTopic.TopicUri, out rawTopic);
                        rawTopic.Dispose();
                    }
                }
            }
        }

        private RawWampTopic<TMessage> CreateRawTopic(string topicUri)
        {
            RawWampTopic<TMessage> newTopic =
                new RawWampTopic<TMessage>(topicUri,
                                           mEventSerializer,
                                           mBinding);

            long subscriptionId =
                mSubscriptionIdToTopic.Add(newTopic);

            newTopic.SubscriptionId = subscriptionId;

            mTopicUriToTopic.TryAdd(topicUri, newTopic);

            newTopic.TopicEmpty += OnTopicEmpty;

            return newTopic;
        }
    }
}