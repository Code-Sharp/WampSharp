using System.Reactive.Subjects;

namespace WampSharp.PubSub
{
    public interface IWampPubSubClientFactory
    {
        ISubject<TEvent> GetSubject<TEvent>(string topicUri);
    }
}
