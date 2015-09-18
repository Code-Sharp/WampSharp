using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSubscriptionDescriptor
    {
        [WampProcedure("wamp.subscription.list")]
        AvailableSubscriptions GetAllSubscriptionIds();

        [WampProcedure("wamp.subscription.lookup")]
        long LookupSubscriptionId(string topicUri, SubscribeOptions options = null);

        [WampProcedure("wamp.subscription.match")]
        long[] GetMatchingSubscriptionIds(string topicUri);

        [WampProcedure("wamp.subscription.get")]
        SubscriptionDetails GetSubscriptionDetails(long subscriptionId);

        [WampProcedure("wamp.subscription.list_subscribers")]
        long[] GetSubscribers(long subscriptionId);

        [WampProcedure("wamp.subscription.count_subscribers")]
        long CountSubscribers(long subscriptionId);
    }
}