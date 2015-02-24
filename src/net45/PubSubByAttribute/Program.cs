using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using WampSharp.Core.Listener;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.DelegatePubSub;
using WampSharp.V2.PubSub;
using WampSharp.V2.Transports;

namespace PubSubByAttribute
{
    class Program
    {
        public static void MyMethod(IDictionary<string, object> arguments)
        {
        
    
        }

        static void Main(string[] args)
        {
            List<string> typeNames = new List<string>();

            for (int i = 0; i <= 16; i++)
            {
                string actionName = string.Format("typeof(Action<{0}>)", string.Join(",", Enumerable.Range(0, i).Select(x => string.Empty)));
                typeNames.Add(actionName);
            }

            string arrayInit = string.Join(", ", typeNames);

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

            WampPublisherRegistrar registrar = new WampPublisherRegistrar(channel2.RealmProxy);
            MyPublisher myPublisher = new MyPublisher();
            registrar.RegisterPublisher(myPublisher, new PublisherRegistrationInterceptor());

            WampSubscriberRegistrar registarer2 = new WampSubscriberRegistrar(channel.RealmProxy);

            ISubscriberRegistrationInterceptor s = new SubscriberRegistrationInterceptor();
            registarer2.RegisterSubscriber(new MySubscriber(), s);

            myPublisher.OnMyAction("Yo", "cool!", 5);

            registrar.UnregisterPublisher(myPublisher, new PublisherRegistrationInterceptor());
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

    internal class MySubscriber
    {
        [WampTopic("com.topic.arguments")]
        public void HandleEvent(string name, string surname, int age)
        {
            
        }
    }

    internal class MyPublisher
    {
        [WampTopic("com.topic.arguments")]
        public event Action<string, string, int> MyAction;

        public virtual void OnMyAction(string arg1, string arg2, int arg3)
        {
            var handler = MyAction;
            if (handler != null) handler(arg1, arg2, arg3);
        }
    }
}
