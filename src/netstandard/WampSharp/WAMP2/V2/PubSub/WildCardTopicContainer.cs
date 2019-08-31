using System.Collections.Generic;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class WildCardTopicContainer : MatchTopicContainer
    {
        private readonly IDictionary<string, WildCardMatcher> mWildCardToEvaluator =
            new SwapDictionary<string, WildCardMatcher>();

        public WildCardTopicContainer(WampIdMapper<IWampTopic> subscriptionIdToTopic) : 
            base(subscriptionIdToTopic)
        {
            TopicCreated += OnTopicCreated;
            TopicRemoved += OnTopicRemoved;
        }

        private void OnTopicCreated(object sender, WampTopicCreatedEventArgs e)
        {
            string wildcard = e.Topic.TopicUri;

            mWildCardToEvaluator[wildcard] = new WildCardMatcher(wildcard);
        }

        private void OnTopicRemoved(object sender, WampTopicRemovedEventArgs e)
        {
            string wildcard = e.Topic.TopicUri;

            mWildCardToEvaluator.Remove(wildcard);
        }

        public override IWampCustomizedSubscriptionId GetSubscriptionId(string topicUri, SubscribeOptions options)
        {
            return new WildCardSubscriptionId(topicUri);
        }

        public override IEnumerable<IWampTopic> GetMatchingTopics(string criteria, PublishOptions publishOptions)
        {
            string[] uriParts = criteria.Split('.');

            foreach (var wildcardToEvaluator in mWildCardToEvaluator)
            {
                string wildcard = wildcardToEvaluator.Key;
                WildCardMatcher evaluator = wildcardToEvaluator.Value;

                if (evaluator.IsMatched(uriParts))
                {
                    IWampTopic topic = GetTopicByUri(wildcard);

                    if (topic != null)
                    {
                        yield return topic;
                    }
                }
            }
        }

        public override bool Handles(SubscribeOptions options)
        {
            return options.Match == WampMatchPattern.Wildcard;
        }
    }
}