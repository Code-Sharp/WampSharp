using System.Reactive.Subjects;
using WampSharp.Core.Listener;

namespace WampSharp.PubSub.Client
{
    public interface IWampPubSubClientFactory<TMessage>
    {
        ISubject<TEvent> GetSubject<TEvent>(string topicUri, IWampConnection<TMessage> connection);
    }
}
