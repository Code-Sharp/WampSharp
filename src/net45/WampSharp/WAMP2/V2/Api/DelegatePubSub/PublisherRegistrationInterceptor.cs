using System.Reflection;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    public class PublisherRegistrationInterceptor : IPublisherRegistrationInterceptor
    {
        private readonly PublishOptions mPublishOptions;

        public PublisherRegistrationInterceptor()
            : this(new PublishOptions())
        {
        }

        public PublisherRegistrationInterceptor(PublishOptions publishOptions)
        {
            mPublishOptions = publishOptions;
        }

        public virtual bool IsPublisherTopic(EventInfo @event)
        {
            return @event.IsDefined(typeof(WampTopicAttribute));
        }

        public virtual PublishOptions GetPublishOptions(EventInfo @event)
        {
            return mPublishOptions;
        }

        public virtual string GetTopicUri(EventInfo @event)
        {
            return @event.GetCustomAttribute<WampTopicAttribute>().Topic;
        }
    }
}