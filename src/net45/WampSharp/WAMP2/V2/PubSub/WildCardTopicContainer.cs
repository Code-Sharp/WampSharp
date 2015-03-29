using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class WildCardTopicContainer : MatchTopicContainer
    {
        private readonly IDictionary<string, WildCardMatcher> mWildCardToEvaluator =
            new SwapDictionary<string, WildCardMatcher>();

        public WildCardTopicContainer()
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

        protected override IEnumerable<IWampTopic> GetMatchingTopics(string criteria)
        {
            foreach (var wildcardToEvaluator in mWildCardToEvaluator)
            {
                string wildcard = wildcardToEvaluator.Key;
                WildCardMatcher evaluator = wildcardToEvaluator.Value;

                if (evaluator.IsMatched(criteria))
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
            return options.Match == "wildcard";
        }

        private class WildCardMatcher
        {
            private readonly string mWildcard;
            private readonly int[] mNonEmptyPartIndexes;
            private readonly string[] mParts;

            public WildCardMatcher(string wildcard)
            {
                mWildcard = wildcard;

                mParts = wildcard.Split('.');

                mNonEmptyPartIndexes =
                    mParts
                        .Select((part, index) => new {part, index})
                        .Where(x => x.part != string.Empty)
                        .Select(x => x.index).ToArray();
            }

            public bool IsMatched(string uri)
            {
                string[] uriParts = uri.Split('.');

                if (mParts.Length > uriParts.Length)
                {
                    return false;
                }

                if (mNonEmptyPartIndexes.All(index => uriParts[index] == mParts[index]))
                {
                    return true;
                }

                return false;
            }
        }
    }
}