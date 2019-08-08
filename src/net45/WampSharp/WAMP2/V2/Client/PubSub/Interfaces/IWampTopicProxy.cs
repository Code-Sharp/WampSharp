using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents a proxy to a WAMP topic.
    /// </summary>
    public interface IWampTopicProxy
    {
        /// <summary>
        /// The topic uri
        /// </summary>
        string TopicUri { get; }

        /// <summary>
        /// Publishes an event to the current topic.
        /// </summary>
        /// <param name="options">The options to publish with.</param>
        /// <returns>A task which is completed when the publish is done, with
        /// the publication id.</returns>
        Task<long?> Publish(PublishOptions options);

        /// <summary>
        /// Publishes an event to the current topic.
        /// </summary>
        /// <param name="options">The options to publish with.</param>
        /// <param name="arguments">The arguments of the published event.</param>
        /// <returns>A task which is completed when the publish is done, with
        /// the publication id.</returns>
        Task<long?> Publish(PublishOptions options, object[] arguments);

        /// <summary>
        /// Publishes an event to the current topic.
        /// </summary>
        /// <param name="options">The options to publish with.</param>
        /// <param name="arguments">The arguments of the published event.</param>
        /// <param name="argumentKeywords">The argument keywords of the published event.</param>
        /// <returns>A task which is completed when the publish is done, with
        /// the publication id.</returns>
        Task<long?> Publish(PublishOptions options, object[] arguments, IDictionary<string, object> argumentKeywords);

        /// <summary>
        /// Subscribes to the the current topic.
        /// </summary>
        /// <param name="subscriber">The subscriber to receive the topic's events.</param>
        /// <param name="options">The options to subscribe with.</param>
        /// <returns>A task that completes when the subscription is done,
        /// with a disposable that its dispose will remove the subscription.</returns>
        Task<IAsyncDisposable> Subscribe(IWampRawTopicClientSubscriber subscriber, SubscribeOptions options);
    }
}