using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSubscriptionDescriptorProxy : IWampSubscriptionDescriptor
    {
        /// <summary>
        /// Retrieves subscription IDs listed according to match policies.
        /// </summary>
        /// <returns>An object with a list of subscription IDs for each match policy.</returns>
        [WampProcedure("wamp.subscription.list")]
        Task<AvailableGroups> GetAllSubscriptionIdsAsync();

        /// <summary>
        /// Obtains the subscription Async(if any) managing a topic, according to some match policy.
        /// </summary>
        /// <param name="topicUri">The URI of the topic.</param>
        /// <param name="options">Same options as when subscribing to a topic.</param>
        /// <returns>The ID of the subscription managing the topic, if found, or null.</returns>
        [WampProcedure("wamp.subscription.lookup")]
        Task<long?> LookupSubscriptionIdAsync(string topicUri, SubscribeOptions options = null);

        /// <summary>
        /// Retrieves a list of IDs of subscriptions matching a topic URI, irrespective of match policy.
        /// </summary>
        /// <param name="topicUri">The topic to match.</param>
        /// <returns>A list of all matching subscription IDs, or null</returns>
        [WampProcedure("wamp.subscription.match")]
        Task<long[]> GetMatchingSubscriptionIdsAsync(string topicUri);

        /// <summary>
        /// Retrieves information on a particular subscription.
        /// </summary>
        /// <param name="subscriptionId">The ID of the subscription to retrieve.</param>
        /// <returns>Details on the subscription.</returns>
        [WampProcedure("wamp.subscription.get")]
        Task<SubscriptionDetails> GetSubscriptionDetailsAsync(long subscriptionId);

        /// <summary>
        /// Retrieves a list of session IDs for sessions currently attached to the subscription.
        /// </summary>
        /// <param name="subscriptionId">The ID of the subscription to get subscribers for.</param>
        /// <returns>A list of WAMP session IDs of subscribers currently attached to the subscription.</returns>
        [WampProcedure("wamp.subscription.list_subscribers")]
        Task<long[]> GetSubscribersAsync(long subscriptionId);

        /// <summary>
        /// Obtains the number of sessions currently attached to a subscription.
        /// </summary>
        /// <param name="subscriptionId">The ID of the subscription to get the number of subscribers for.</param>
        /// <returns>The number of sessions currently attached to a subscription.</returns>
        [WampProcedure("wamp.subscription.count_subscribers")]
        Task<long> CountSubscribersAsync(long subscriptionId);
    }
}