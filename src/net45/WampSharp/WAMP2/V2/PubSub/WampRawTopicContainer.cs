using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using WampSharp.Core.Utilities;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class WampRawTopicContainer<TMessage> : IWampRawTopicContainer<TMessage>
    {
        private readonly IWampTopicContainer mTopicContainer;
        private readonly IWampEventSerializer mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly object mLock = new object();

        private readonly ConcurrentDictionary<long, WampRawTopic<TMessage>> mSubscriptionIdToTopic =
            new ConcurrentDictionary<long, WampRawTopic<TMessage>>();

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

                IWampCustomizedSubscriptionId customizedSubscriptionId =
                    mTopicContainer.GetSubscriptionId(topicUri, options);

                if (!mTopicUriToTopic.TryGetValue(customizedSubscriptionId, out WampRawTopic<TMessage> rawTopic))
                {
                    rawTopic = CreateRawTopic(topicUri, options, customizedSubscriptionId);

                    IWampRegistrationSubscriptionToken subscriptionToken =
                        mTopicContainer.Subscribe(rawTopic, topicUri, options);

                    long subscriptionId =
                        subscriptionToken.TokenId;

                    mSubscriptionIdToTopic.TryAdd(subscriptionId, rawTopic);

                    rawTopic.SubscriptionId = subscriptionId;

                    rawTopic.SubscriptionDisposable = subscriptionToken;
                }

                rawTopic.Subscribe(request, options);
                
                return rawTopic.SubscriptionId;
            }
        }

        public void Unsubscribe(IUnsubscribeRequest<TMessage> request, long subscriptionId)
        {
            lock (mLock)
            {

                if (!mSubscriptionIdToTopic.TryGetValue(subscriptionId, out WampRawTopic<TMessage> rawTopic))
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

            if (sender is WampRawTopic<TMessage> rawTopic)
            {
                lock (mLock)
                {
                    if (!rawTopic.HasSubscribers)
                    {
                        mSubscriptionIdToTopic.TryRemoveExact(rawTopic.SubscriptionId, rawTopic);
                        mTopicUriToTopic.TryRemoveExact(rawTopic.CustomizedSubscriptionId, rawTopic);
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

            mTopicUriToTopic.TryAdd(customizedSubscriptionId, newTopic);

            newTopic.TopicEmpty += OnTopicEmpty;

            return newTopic;
        }
    }
}