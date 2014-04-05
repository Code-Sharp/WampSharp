using System;

namespace WampSharp.V2.PubSub
{
    public interface ISubscriptionNotifier
    {
        event EventHandler<SubscriptionAddEventArgs> SubscriptionAdding;
        event EventHandler<SubscriptionAddEventArgs> SubscriptionAdded;

        event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoving;
        event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoved;

        event EventHandler TopicEmpty;
    }
}