using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class ExactTopicContainer : MatchTopicContainer
    {
        public override IWampCustomizedSubscriptionId GetSubscriptionId(string topicUri, SubscribeOptions options)
        {
            return new ExactTopicSubscriptionId(topicUri);
        }

        protected override IEnumerable<IWampTopic> GetMatchingTopics(string criteria)
        {
            IWampTopic topic = GetTopicByUri(criteria);

            if (topic == null)
            {
                yield break;
            }

            yield return topic;
        }

        public override bool Handles(SubscribeOptions options)
        {
            return options.Match == null || options.Match == "exact";
        }
    }
}