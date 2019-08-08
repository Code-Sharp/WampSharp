using System;
using System.Threading.Tasks;
using WampSharp.V2.Client;

namespace WampSharp.V2.MetaApi
{
    public class SubscriptionEvents : MetaApiEventsBase<IWampSubscriptionMetadataSubscriber>
    {
        /// <summary>
        /// Fired when a subscription is created through a subscription request for a topic 
        /// which was previously without subscribers.
        /// </summary>
        /// <param name="sessionId">ID of the session performing the subscription request.</param>
        /// <param name="details">Information on the created subscription.</param>
        public delegate void OnCreateDelegate(long sessionId, SubscriptionDetails details);

        /// <summary>
        /// Fired when a session is added to a subscription.
        /// </summary>
        /// <param name="sessionId">ID of the session being added to a subscription.</param>
        /// <param name="subscriptionId">ID of the subscription to which the session is being added.</param>
        public delegate void OnSubscribeDelegate(long sessionId, long subscriptionId);

        /// <summary>
        /// Fired when a session is removed from a subscription.
        /// </summary>
        /// <param name="sessionId">ID of the session being removed from a subscription.</param>
        /// <param name="subscriptionId">ID of the subscription from which the session is being removed.</param>
        public delegate void OnUnsubscribeDelegate(long sessionId, long subscriptionId);

        /// <summary>
        /// Fired when a subscription is deleted after the last session attached to it has been removed.
        /// </summary>
        /// <param name="sessionId">ID of the last session being removed from a subscription.</param>
        /// <param name="subscriptionId">ID of the subscription being deleted.</param>
        public delegate void OnDeleteDelegate(long sessionId, long subscriptionId);

        public SubscriptionEvents(IWampRealmProxy realmProxy) : base(realmProxy)
        {
        }

        public Task<IAsyncDisposable> OnCreate(OnCreateDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnCreate(default(long), default(SubscriptionDetails)));
        }

        public Task<IAsyncDisposable> OnDelete(OnDeleteDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnDelete(default(long), default(long)));
        }

        public Task<IAsyncDisposable> OnSubscribe(OnSubscribeDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnSubscribe(default(long), default(long)));
        }

        public Task<IAsyncDisposable> OnUnsubscribe(OnUnsubscribeDelegate handler)
        {
            return InnerSubscribe(handler, x => x.OnUnsubscribe(default(long), default(long)));
        }
    }
}