using System;
using System.Reactive.Subjects;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.Samples.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverAddress = "ws://127.0.0.1:8080/ws";

            ClientCode(serverAddress);

            Console.ReadLine();
        }

        private static void ClientCode(string serverAddress)
        {
            DefaultWampChannelFactory channelFactory = 
                new DefaultWampChannelFactory();

            IWampChannel wampChannel =
                channelFactory.CreateJsonChannel(serverAddress, "realm1");

            wampChannel.Open().Wait();

                wampChannel.RealmProxy.Services.RegisterSubscriber(new MyHandler(), new SubscriberRegistrationInterceptor(new SubscribeOptions()
                {
                    GetRetained = true
                }));
        }

        private static IDisposable ServerCode(WampHost host)
        {
            IWampHostedRealm realm = host.RealmContainer.GetRealmByName("realm1");

            IWampTopicContainer topicContainer = realm.TopicContainer;

            IWampTopic topic =
                topicContainer.GetOrCreateTopicByUri("com.myapp.topic1");

            IDisposable disposable = realm.Services.GetSubject<int>("com.myapp.topic1")
                                          .Subscribe(x => GetValue(x));

            return disposable;
        }

        private static void GetValue(int number)
        {
            Console.WriteLine("Got " + number);
        }
    }

    internal class MyHandler
    {
        [WampTopic("com.myapp.topic1")]
        public void OnCounter(int value)
        {
            Console.WriteLine(value);
        }
    }
}