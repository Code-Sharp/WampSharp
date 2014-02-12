using System.Reactive.Subjects;
using WampSharp.Core.Listener;

namespace WampSharp.V1.PubSub.Client
{
    /// <summary>
    /// An interface that allows to consume pub/sub client capabilities.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampPubSubClientFactory<TMessage>
    {
        /// <summary>
        /// Gets a subject proxy of a given topic uri.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="topicUri">The given topic uri.</param>
        /// <param name="connection">The connection the messages are sent through.</param>
        /// <returns>The requested subject.</returns>
        ISubject<TEvent> GetSubject<TEvent>(string topicUri, IWampConnection<TMessage> connection);
    }
}
