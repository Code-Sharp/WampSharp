using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal class PrefixTopicContainer : MatchTopicContainer
    {
        public PrefixTopicContainer(WampIdMapper<IWampTopic> subscriptionIdToTopic) : 
            base(subscriptionIdToTopic)
        {
        }

        public override IWampCustomizedSubscriptionId GetSubscriptionId(string topicUri, SubscribeOptions options)
        {
            return new PrefixSubscriptionId(topicUri);
        }

        public override IEnumerable<IWampTopic> GetMatchingTopics(string criteria, PublishOptions publishOptions)
        {
            return this.Topics.Where(x => criteria.StartsWith(x.TopicUri));
        }

        public override bool Handles(SubscribeOptions options)
        {
            return options.Match == WampMatchPattern.Prefix;
        }
    }
}