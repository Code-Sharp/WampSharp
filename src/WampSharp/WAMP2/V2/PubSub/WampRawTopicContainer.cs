using System.Collections.Concurrent;
using System.Linq;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    // TODO: this class is not complete, review and fix this.
    public class WampRawTopicContainer<TMessage> : IWampRawTopicContainer<TMessage>
    {
        private IWampIdGenerator mGenerator = new WampIdGenerator();
        private IWampTopicContainer mTopicContainer;
        
        private readonly ConcurrentDictionary<string, RawWampTopic<TMessage>> mTopicUriToRawTopic = 
            new ConcurrentDictionary<string, RawWampTopic<TMessage>>();

        private readonly ConcurrentDictionary<long, RawWampTopic<TMessage>> mSubscriptionIdToRawTopic =
            new ConcurrentDictionary<long, RawWampTopic<TMessage>>();

        private readonly IWampEventSerializer<TMessage> mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;

        public WampRawTopicContainer(IWampTopicContainer topicContainer, IWampEventSerializer<TMessage> eventSerializer, IWampBinding<TMessage> binding)
        {
            mTopicContainer = topicContainer;
            mEventSerializer = eventSerializer;
            mBinding = binding;
        }

        public long Subscribe(IWampSubscriber subscriber, TMessage options, string topicUri)
        {
            RawWampTopic<TMessage> rawTopic;
            if (mTopicUriToRawTopic.TryGetValue(topicUri, out rawTopic))
            {
                rawTopic.Subscribe(subscriber, options);
            }
            else
            {
                // TODO: subscribe to the real topic, from the topic container
                rawTopic = 
                    new RawWampTopic<TMessage>(topicUri, 
                        mGenerator.Generate(),
                        mEventSerializer,
                        mBinding);

                mTopicUriToRawTopic.TryAdd(topicUri, rawTopic);
                mSubscriptionIdToRawTopic.TryAdd(rawTopic.SubscriptionId, rawTopic);
            }

            return rawTopic.SubscriptionId;
        }

        public void Unsubscribe(IWampSubscriber subscriber, long subscriptionId)
        {
            RawWampTopic<TMessage> rawTopic;

            if (mSubscriptionIdToRawTopic.TryGetValue(subscriptionId, out rawTopic))
            {
                rawTopic.Unsubscribe(subscriber);
            }
            else
            {
                throw new WampException(WampErrors.NoSuchSubscription, subscriptionId);
            }
        }

        public long Publish(TMessage options, string topicUri)
        {
            // TODO: publish to the real topic
            RawWampTopic<TMessage> rawTopic = GetTopic(topicUri);

            long publicationId = mGenerator.Generate();

            rawTopic.Event(publicationId, options);
         
            return publicationId;
        }

        public long Publish(TMessage options, string topicUri, TMessage[] arguments)
        {
            // TODO: publish to the real topic
            RawWampTopic<TMessage> rawTopic = GetTopic(topicUri);

            long publicationId = mGenerator.Generate();

            rawTopic.Event(publicationId, options, arguments.Cast<object>().ToArray());

            return publicationId;
        }

        public long Publish(TMessage options, string topicUri, TMessage[] arguments, TMessage argumentKeywords)
        {
            // TODO: publish to the real topic
            RawWampTopic<TMessage> rawTopic = GetTopic(topicUri);

            long publicationId = mGenerator.Generate();

            rawTopic.Event(publicationId, options, arguments.Cast<object>().ToArray(), argumentKeywords);

            return publicationId;
        }

        private RawWampTopic<TMessage> GetTopic(string topicUri)
        {
            RawWampTopic<TMessage> rawWampTopic;

            if (!mTopicUriToRawTopic.TryGetValue(topicUri, out rawWampTopic))
            {
                throw new WampException(WampErrors.InvalidTopic, topicUri);
            }

            return rawWampTopic;
        }

    }
}