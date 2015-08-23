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
        private readonly IWampEventSerializer mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly object mLock = new object();

        private readonly WampIdMapper<WampRawTopic<TMessage>> mSubscriptionIdToTopic =
            new WampIdMapper<WampRawTopic<TMessage>>();

        private readonly ConcurrentDictionary<IWampCustomizedSubscriptionId, WampRawTopic<TMessage>> mTopicUriToTopic =
            new ConcurrentDictionary<IWampCustomizedSubscriptionId, WampRawTopic<TMessage>>();

        public WampRawTopicContainer(IWampTopicContainer topicContainer,
                                     IWampEventSerializer eventSerializer,
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
                WampRawTopic<TMessage> rawTopic;

                IWampCustomizedSubscriptionId customizedSubscriptionId =
                    mTopicContainer.GetSubscriptionId(topicUri, options);

                if (!mTopicUriToTopic.TryGetValue(customizedSubscriptionId, out rawTopic))
                {
                    rawTopic = CreateRawTopic(topicUri, options, customizedSubscriptionId);

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
                WampRawTopic<TMessage> rawTopic;

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
            WampRawTopic<TMessage> rawTopic = sender as WampRawTopic<TMessage>;

            if (rawTopic != null)
            {
                lock (mLock)
                {
                    if (!rawTopic.HasSubscribers)
                    {
                        WampRawTopic<TMessage> rawTopicIgnore;
                        mSubscriptionIdToTopic.TryRemove(rawTopic.SubscriptionId, out rawTopicIgnore); // "out rawTopic" makes it null in some cases
                        mTopicUriToTopic.TryRemove(rawTopic.CustomizedSubscriptionId, out rawTopicIgnore); // "out rawTopic" makes it null in some cases
                        rawTopic.Dispose();
                    }
                }
            }
        }

        private WampRawTopic<TMessage> CreateRawTopic(string topicUri, SubscribeOptions subscriptionOptions, IWampCustomizedSubscriptionId customizedSubscriptionId)
        {
            WampRawTopic<TMessage> newTopic =
                new WampRawTopic<TMessage>(topicUri,
                                           subscriptionOptions,
                                           customizedSubscriptionId,
                                           mEventSerializer,
                                           mBinding);

            long subscriptionId =
                mSubscriptionIdToTopic.Add(newTopic);

            newTopic.SubscriptionId = subscriptionId;

            mTopicUriToTopic.TryAdd(customizedSubscriptionId, newTopic);

            newTopic.TopicEmpty += OnTopicEmpty;

            return newTopic;
        }
    }
}
