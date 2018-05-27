using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SystemEx;
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

        public Task<IAsyncDisposable> RegisterSubscriber(object instance, ISubscriberRegistrationInterceptor interceptor)
        {
            return AggregateSubscriberEventHandlers(instance, interceptor);
        }

        private Task<IAsyncDisposable> AggregateSubscriberEventHandlers(object instance, ISubscriberRegistrationInterceptor interceptor)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            IEnumerable<Type> typesToExplore = GetTypesToExplore(instance);

            List<Task<IAsyncDisposable>> tasks = new List<Task<IAsyncDisposable>>(); 

            foreach (Type type in typesToExplore)
            {
                foreach (MethodInfo method in type.GetPublicInstanceMethods())
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

                        Task<IAsyncDisposable> task =
                            topicProxy.Subscribe(methodInfoSubscriber, subscribeOptions);

                        tasks.Add(task);
                    }
                }
            }

            Task<IAsyncDisposable> result = tasks.ToAsyncDisposableTask();

            return result;
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