using System;

namespace WampSharp.PubSub.Server
{
    public interface IWampTopicContainerExtended<TMessage> : IWampTopicContainer
    {
        IDisposable Subscribe(string topicUri, IObserver<object> observer);
        void Unsubscribe(string topicUri, string sessionId);
        void Publish(string topicUri, TMessage @event, string[] exclude, string[] eligible);         
    }
}