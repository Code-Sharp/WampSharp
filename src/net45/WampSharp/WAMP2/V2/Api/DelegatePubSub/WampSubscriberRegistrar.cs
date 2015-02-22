using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.DelegatePubSub
{
    public class WampSubscriberRegistrar
    {
        private readonly IWampRealmProxy mProxy;

        public WampSubscriberRegistrar(IWampRealmProxy proxy)
        {
            mProxy = proxy;
        }

        public Task RegisterSubscriber(object instance, ISubscriberRegistrationInterceptor interceptor)
        {
            return AggregateSubscriberEventHandlers(instance, interceptor);
        }

        private Task AggregateSubscriberEventHandlers(object instance, ISubscriberRegistrationInterceptor interceptor)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            IEnumerable<Type> typesToExplore = GetTypesToExplore(instance);

            List<Task> tasks = new List<Task>(); 

            foreach (Type type in typesToExplore)
            {
                foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance |
                                                              BindingFlags.Public))
                {
                    if (interceptor.IsSubscriberHandler(method))
                    {
                        string topicUri = interceptor.GetTopicUri(method);
                        
                        SubscribeOptions subscribeOptions =
                            interceptor.GetSubscribeOptions(method);

                        IWampTopicProxy topicProxy = 
                            mProxy.TopicContainer.GetTopicByUri(topicUri);

                        IWampRawTopicClientSubscriber methodInfoSubscriber = 
                            new MethodInfoSubscriber(instance, method, topicUri);

                        Task task =
                            topicProxy.Subscribe(methodInfoSubscriber, subscribeOptions);

                        tasks.Add(task);
                    }
                }
            }

            return Task.WhenAll(tasks);
        }

        private IEnumerable<Type> GetTypesToExplore(object instance)
        {
            Type runtimeType = instance.GetType();

            yield return runtimeType;

            foreach (Type @interface in runtimeType.GetInterfaces())
            {
                yield return @interface;
            }
        }
    }
}