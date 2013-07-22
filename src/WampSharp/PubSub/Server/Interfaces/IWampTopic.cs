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
        
        event EventHandler<WampSubscriptionAddedEventArgs> SubscriptionAdded;
        event EventHandler<WampSubscriptionRemovedEventArgs> SubscriptionRemoved;

        void Unsubscribe(string sessionId);
    }
}