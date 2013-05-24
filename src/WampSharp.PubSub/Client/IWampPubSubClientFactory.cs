using System.Reactive.Subjects;

namespace WampSharp.PubSub.Client
{
    public interface IWampPubSubClientFactory
    {
        ISubject<TEvent> GetSubject<TEvent>(string topicUri);
    }
}
