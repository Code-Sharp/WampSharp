using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reflection;
using WampSharp.Core.Listener;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.DelegatePubSub
{
    internal class WampPublisherRegistrar
    {
        private readonly IWampRealmProxy mProxy;

        private readonly EventHandlerGenerator mEventHandlerGenerator = new EventHandlerGenerator();

        public WampPublisherRegistrar(IWampRealmProxy proxy)
        {
            mProxy = proxy;
        }

        public IDisposable RegisterPublisher(object instance, IPublisherRegistrationInterceptor interceptor)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            Type runtimeType = instance.GetType();

            IEnumerable<Type> typesToExplore = GetTypesToExplore(runtimeType);

            List<IDisposable> disposables = new List<IDisposable>();

            foreach (Type type in typesToExplore)
            {
                foreach (EventInfo @event in type.GetPublicInstanceEvents())
                {
                    if (interceptor.IsPublisherTopic(@event))
                    {
                        IDisposable disposable = 
                            RegisterToEvent(instance, @event, interceptor);

                        disposables.Add(disposable);
                    }
                }
            }

            IDisposable result = 
                new CompositeDisposable(disposables);

            return result;
        }

        private IEnumerable<Type> GetTypesToExplore(Type type)
        {
            yield return type;

            foreach (Type @interface in type.GetInterfaces())
            {
                yield return @interface;
            }
        }

        private IDisposable RegisterToEvent(object instance, EventInfo @event, IPublisherRegistrationInterceptor interceptor)
        {
            string topic = interceptor.GetTopicUri(@event);

            IWampTopicProxy topicProxy = mProxy.TopicContainer.GetTopicByUri(topic);

            PublishOptions options = interceptor.GetPublishOptions(@event);

            Delegate createdDelegate;

            Type eventHandlerType = @event.EventHandlerType;

            if (IsPositional(eventHandlerType))
            {
                createdDelegate =
                    mEventHandlerGenerator.CreatePositionalDelegate(eventHandlerType, topicProxy, options);
            }
            else
            {
                createdDelegate =
                    mEventHandlerGenerator.CreateKeywordsDelegate(eventHandlerType, topicProxy, options);
            }

            @event.AddEventHandler(instance, createdDelegate);

            PublisherDisposable disposable =
                new PublisherDisposable(instance,
                    createdDelegate,
                    @event,
                    topic, mProxy.Monitor);
            
            return disposable;
        }

        private bool IsPositional(Type eventHandlerType)
        {
            ICollection<Type> actionTypes =
                new Type[]
                {
                    typeof (Action),
                    typeof (Action<>), 
                    typeof (Action<,>), 
                    typeof (Action<,,>), 
                    typeof (Action<,,,>),
                    typeof (Action<,,,,>), 
                    typeof (Action<,,,,,>), 
                    typeof (Action<,,,,,,>), 
                    typeof (Action<,,,,,,,>),
                    typeof (Action<,,,,,,,,>), 
                    typeof (Action<,,,,,,,,,>), 
                    typeof (Action<,,,,,,,,,,>),
                    typeof (Action<,,,,,,,,,,,>), 
                    typeof (Action<,,,,,,,,,,,,>), 
                    typeof (Action<,,,,,,,,,,,,,>),
                    typeof (Action<,,,,,,,,,,,,,,>), 
                    typeof (Action<,,,,,,,,,,,,,,,>)
                };

            // TODO: add support using the interceptor/an attribute.
            if (!eventHandlerType.IsGenericType)
            {
                return actionTypes.Contains(eventHandlerType);
            }
            else
            {
                return actionTypes.Contains(eventHandlerType.GetGenericTypeDefinition());
            }
        }

        private class PublisherDisposable : IDisposable
        {
            private readonly object mInstance;
            private readonly Delegate mDelegate;
            private readonly EventInfo mEvent;
            private readonly string mTopicUri;
            private readonly IWampClientConnectionMonitor mMonitor;

            public PublisherDisposable(object instance, Delegate @delegate, EventInfo @event, string topicUri, IWampClientConnectionMonitor monitor)
            {
                mInstance = instance;
                mDelegate = @delegate;
                mEvent = @event;
                mTopicUri = topicUri;
                mMonitor = monitor;

                monitor.ConnectionBroken += OnConnectionBroken;
                monitor.ConnectionError += OnConnectionError;
            }

            private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
            {
                Dispose();
            }

            private void OnConnectionBroken(object sender, WampSessionCloseEventArgs e)
            {
                Dispose();
            }

            public void Dispose()
            {
                mEvent.RemoveEventHandler(mInstance, mDelegate);
                mMonitor.ConnectionBroken-= OnConnectionBroken;
                mMonitor.ConnectionError -= OnConnectionError;
            }
        }
    }
}