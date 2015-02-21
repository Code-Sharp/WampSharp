using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.Core.Listener;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Client;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.DelegatePubSub;
using WampSharp.V2.Transports;

namespace PubSubByAttribute
{
    public class EventHandlerGenerator
    {
        private static readonly MethodInfo mPublishArrayMethod =
            Method.Get((IWampTopicProxy topicProxy) =>
                topicProxy.Publish(default(PublishOptions), default(object[])));

        private static readonly MethodInfo mPublishArgumentsMethod =
            Method.Get((IWampTopicProxy topicProxy) =>
                topicProxy.Publish(default(PublishOptions), default(object[]), default(IDictionary<string, object>)));

        private static readonly MethodInfo mAddMethod =
            Method.Get((IDictionary<string, object> dictionary) =>
                dictionary.Add(default(string), default(object)));

        public TDelegate CreateArrayDelegate<TDelegate>(IWampTopicProxy proxy)
        {
            Func<ParameterExpression[], Expression> callExpression = parameters =>
                Expression.Call(Expression.Constant(proxy), mPublishArrayMethod,
                    Expression.Constant(new PublishOptions() {}),
                    Expression.NewArrayInit(typeof (object),
                        parameters.Select(x => Expression.Convert(x, typeof (object)))));

            return InnerCreateDelegate<TDelegate>(callExpression);
        }

        public TDelegate CreateArgumentsDelegate<TDelegate>(IWampTopicProxy proxy)
        {
            Func<ParameterExpression[], Expression> callExpression = parameters =>
                Expression.Call(Expression.Constant(proxy), mPublishArgumentsMethod,
                    Expression.Constant(new PublishOptions() {}),
                    Expression.NewArrayInit(typeof (object), Enumerable.Empty<Expression>()),
                    Expression.ListInit(Expression.New(typeof (Dictionary<string, object>)),
                        parameters.Select(x => Expression.ElementInit(mAddMethod,
                            Expression.Constant(x.Name, typeof (string)),
                            Expression.Convert(x, typeof (object))))
                        ));

            return InnerCreateDelegate<TDelegate>(callExpression);
        }

        private static TDelegate InnerCreateDelegate<TDelegate>(Func<ParameterExpression[], Expression> callPublishFactory)
        {
            MethodInfo method =
                typeof (TDelegate).GetMethod("Invoke");

            ParameterExpression[] parameters =
                method.GetParameters()
                    .Select(x => Expression.Parameter(x.ParameterType, x.Name))
                    .ToArray();

            Expression callPublish = callPublishFactory(parameters);

            Expression<TDelegate> lambda =
                Expression.Lambda<TDelegate>(callPublish, parameters);

            TDelegate result = lambda.Compile();

            return result;
        }

        private bool IsAction(Type type)
        {
            return type.Name == "Action";
        }

        private static class Method
        {
            public static MethodInfo Get<T>(Expression<Action<T>> methodCall)
            {
                MethodCallExpression callExpression = methodCall.Body as MethodCallExpression;

                return callExpression.Method;
            }
        }
    }

    class Program
    {
        public static void MyMethod(IDictionary<string, object> arguments)
        {
            
        }

        static void Main(string[] args)
        {

            var init =
                new Expression[]
                {
                    Expression.ListInit(Expression.New(typeof (Dictionary<string, object>)), new ElementInit[]
                    {
                        Expression.ElementInit(typeof (Dictionary<string, object>).GetMethod("Add"),
                            new Expression[]
                            {
                                Expression.Constant("Key", typeof (string)),
                                Expression.Convert(Expression.Constant(3, typeof (int)), typeof (object))
                            })
                    })
                };


            Expression<Action> a =
                () => MyMethod(new Dictionary<string, object>() {{"Key", 3}});

            WampHost host = new WampHost();
            InMemoryTransport inMemoryTransport = new InMemoryTransport(Scheduler.Immediate);
            IWampBinding<object> inMemoryBinding = new InMemoryBinding();
            host.RegisterTransport(inMemoryTransport, new IWampBinding[] {inMemoryBinding});
            host.Open();

            IWampChannel channel = CreateClient(inMemoryTransport, inMemoryBinding);
            IWampChannel channel2 = CreateClient(inMemoryTransport, inMemoryBinding);


            channel.Open();
            channel2.Open();

            IWampSubject subject = 
                channel.RealmProxy.Services.GetSubject("com.topic.arguments");

            subject.Subscribe(x =>
            {

            });

            IWampTopicProxy topicProxy = 
                channel2.RealmProxy.TopicContainer.GetTopicByUri("com.topic.arguments");

            EventHandlerGenerator generator = new EventHandlerGenerator();

            Action<string, int, int> action =
                PublisherDelegateFactory<Action<string, int, int>>.CreateArgumentsHandler(topicProxy,
                    new PublishOptions());

            Action<string, int, int> action2 =
                PublisherDelegateFactory<Action<string, int, int>>.CreateArrayHandler(topicProxy,
                    new PublishOptions());


            action("Hello", 5, 4);
            action2("Hello", 5, 4);
        }

        private static IWampChannel CreateClient(InMemoryTransport inMemoryTransport, IWampBinding<object> inMemoryBinding)
        {
            IControlledWampConnection<object> controlledWampConnection =
                inMemoryTransport.CreateClientConnection(inMemoryBinding, Scheduler.Immediate);

            WampChannelFactory factory = new WampChannelFactory();
            IWampChannel channel = factory.CreateChannel("realm1", controlledWampConnection, inMemoryBinding);
            return channel;
        }
    }
}
