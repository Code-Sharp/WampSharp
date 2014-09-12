using System;

namespace WampSharp.V2.PubSub
{
    public interface ISubscriptionNotifier
    {
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;

        event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoving;
        event EventHandler<SubscriptionRemoveEventArgs> SubscriptionRemoved;

        event EventHandler TopicEmpty;
    }
}