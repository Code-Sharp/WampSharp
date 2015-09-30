using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSubscriptionMetadataSubscriber
    {
        /// <summary>
        /// Fired when a subscription is created through a subscription request for a topic 
        /// which was previously without subscribers.
        /// </summary>
        /// <param name="sessionId">ID of the session performing the subscription request.</param>
        /// <param name="details">Information on the created subscription.</param>
        [WampTopic("wamp.subscription.on_create")]
        void OnCreate(long sessionId, SubscriptionDetails details);

        /// <summary>
        /// Fired when a session is added to a subscription.
        /// </summary>
        /// <param name="sessionId">ID of the session being added to a subscription.</param>
        /// <param name="subscriptionId">ID of the subscription to which the session is being added.</param>
        [WampTopic("wamp.subscription.on_subscribe")]
        void OnSubscribe(long sessionId, long subscriptionId);

        /// <summary>
        /// Fired when a session is removed from a subscription.
        /// </summary>
        /// <param name="sessionId">ID of the session being removed from a subscription.</param>
        /// <param name="subscriptionId">ID of the subscription from which the session is being removed.</param>
        [WampTopic("wamp.subscription.on_unsubscribe")]
        void OnUnsubscribe(long sessionId, long subscriptionId);

        /// <summary>
        /// Fired when a subscription is deleted after the last session attached to it has been removed.
        /// </summary>
        /// <param name="sessionId">ID of the last session being removed from a subscription.</param>
        /// <param name="subscriptionId">ID of the subscription being deleted.</param>
        [WampTopic("wamp.subscription.on_delete")]
        void OnDelete(long sessionId, long subscriptionId);
    }
}