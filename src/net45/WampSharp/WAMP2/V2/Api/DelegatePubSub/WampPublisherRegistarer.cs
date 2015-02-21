using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.DelegatePubSub
{
    public class WampPublisherRegistarer
    {
        private readonly IWampRealmProxy mProxy;

        private readonly EventHandlerGenerator mEventHandlerGenerator = new EventHandlerGenerator();

        private readonly ConcurrentDictionary<Tuple<object, string>, PublisherRegistration> mRegistrations =
            new ConcurrentDictionary<Tuple<object, string>, PublisherRegistration>();

        public WampPublisherRegistarer(IWampRealmProxy proxy)
        {
            mProxy = proxy;
        }

        public void RegisterPublisher(object instance, IPublisherRegistrationInterceptor interceptor)
        {
            AggregateTopics(instance, interceptor, RegisterToEvent);
        }

        public void UnregisterPublisher(object instance, IPublisherRegistrationInterceptor interceptor)
        {
            AggregateTopics(instance, interceptor, UnregisterFromEvent);
        }

        private void AggregateTopics(object instance, IPublisherRegistrationInterceptor interceptor, Action<object, EventInfo, IPublisherRegistrationInterceptor> action)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Type runtimeType = instance.GetType();

            IEnumerable<Type> typesToExplore = GetTypesToExplore(runtimeType);

            foreach (Type type in typesToExplore)
            {
                foreach (EventInfo @event in type.GetEvents(BindingFlags.Instance |
                                                            BindingFlags.Public |
                                                            BindingFlags.NonPublic))
                {
                    if (interceptor.IsPublisherTopic(@event))
                    {
                        action(instance, @event, interceptor);
                    }
                }
            }
        }

        private IEnumerable<Type> GetTypesToExplore(Type type)
        {
            yield return type;

            foreach (Type @interface in type.GetInterfaces())
            {
                yield return @interface;
            }
        }

        private void RegisterToEvent(object instance, EventInfo @event, IPublisherRegistrationInterceptor interceptor)
        {
            string topic = interceptor.GetTopicUri(@event);

            IWampTopicProxy topicProxy = mProxy.TopicContainer.GetTopicByUri(topic);
            PublishOptions options = interceptor.GetPublishOptions(@event);

            Delegate createdDelegate;

            Type eventHandlerType = @event.EventHandlerType;

            if (IsPositional(eventHandlerType))
            {
                createdDelegate =
                    mEventHandlerGenerator.CreateKeywordsDelegate(eventHandlerType, topicProxy, options);
            }
            else
            {
                createdDelegate =
                    mEventHandlerGenerator.CreatePositionalDelegate(eventHandlerType, topicProxy, options);
            }

            @event.AddEventHandler(instance, createdDelegate);

            PublisherRegistration registration = new PublisherRegistration(instance, createdDelegate, @event, topic);

            mRegistrations.TryAdd(Tuple.Create(instance, topic), registration);
        }

        private bool IsPositional(Type eventHandlerType)
        {
            // TODO: add support using the interceptor/an attribute.
            return eventHandlerType.Name == typeof (Action).Name &&
                   eventHandlerType.Namespace == typeof (Action).Namespace &&
                   eventHandlerType.Assembly == typeof (Action).Assembly ||
                   eventHandlerType.Assembly == typeof (Action<,,,,,,,,,,,,,,,>).Assembly;
        }

        private void UnregisterFromEvent(object instance, EventInfo @event, IPublisherRegistrationInterceptor interceptor)
        {
            string topic = interceptor.GetTopicUri(@event);

            PublisherRegistration registration;

            if (mRegistrations.TryRemove(Tuple.Create(instance, topic), out registration))
            {
                registration.Dispose();
            }
        }

        private class PublisherRegistration : IDisposable
        {
            private readonly object mInstance;
            private readonly Delegate mDelegate;
            private readonly EventInfo mEvent;
            private readonly string mTopicUri;

            public PublisherRegistration(object instance, Delegate @delegate, EventInfo @event, string topicUri)
            {
                mInstance = instance;
                mDelegate = @delegate;
                mEvent = @event;
                mTopicUri = topicUri;
            }

            public void Dispose()
            {
                mEvent.RemoveEventHandler(mInstance, mDelegate);
            }
        }
    }
}