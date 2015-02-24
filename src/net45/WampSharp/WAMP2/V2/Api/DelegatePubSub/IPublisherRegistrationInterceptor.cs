using System.Reflection;
using WampSharp.V2.Core.Contracts;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    /// <summary>
    /// Represents an interface that allows to get involved in publisher registration.
    /// </summary>
    public interface IPublisherRegistrationInterceptor
    {
        /// <summary>
        /// Returns a value indicating whether this event represents a publisher topic.
        /// </summary>
        bool IsPublisherTopic(EventInfo @event);

        /// <summary>
        /// Gets the options that will be used to publish to the given event.
        /// </summary>
        PublishOptions GetPublishOptions(EventInfo @event);

        /// <summary>
        /// Gets the topic uri that will be used to publish to the given event.
        /// </summary>
        string GetTopicUri(EventInfo @event);
    }
}