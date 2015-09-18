using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public interface IWampSubscriptionMetadataSubscriber
    {
        [WampTopic("wamp.subscription.on_create")]
        void OnCreate(long sessionId, SubscriptionDetails details);

        [WampTopic("wamp.subscription.on_subscribe")]
        void OnSubscribe(long sessionId, long subscriptionId);

        [WampTopic("wamp.subscription.on_unsubscribe")]
        void OnUnsubscribe(long sessionId, long subscriptionId);

        [WampTopic("wamp.subscription.on_delete")]
        void OnDelete(long sessionId, long subscriptionId);
    }
}