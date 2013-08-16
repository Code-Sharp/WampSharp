using System;
using System.Reactive.Subjects;

namespace WampSharp.PubSub.Server
{
    public interface IWampTopic :
        ISubject<object>, 
        ISubject<WampNotification, object>,
        IDisposable
    {
        string TopicUri { get; }
        bool Persistent { get; }

        bool HasObservers { get; }

        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdding;
        event EventHandler<WampSubscriptionAddEventArgs> SubscriptionAdded;
        event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoving;
        event EventHandler<WampSubscriptionRemoveEventArgs> SubscriptionRemoved;

        event EventHandler TopicEmpty; 

        void Unsubscribe(string sessionId);
    }
}