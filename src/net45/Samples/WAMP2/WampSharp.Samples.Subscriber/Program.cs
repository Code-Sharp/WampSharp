using System;
using System.Reactive.Subjects;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;
using WampSharp.WebSocket4Net;

namespace WampSharp.Samples.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverAddress = "ws://127.0.0.1:8080/ws";
            
            WampHost host = new DefaultWampHost(serverAddress);

            //IDisposable disposable = ServerCode(host);

            host.Open();

            IDisposable disposable = ClientCode(serverAddress);

            Console.ReadLine();

            disposable.Dispose();

            Console.ReadLine();
        }

        private static IDisposable ClientCode(string serverAddress)
        {
            JTokenBinding jTokenBinding = new JTokenBinding();

            WampChannelFactory channelFactory = 
                new WampChannelFactory();

            IWampChannel wampChannel =
                channelFactory.CreateChannel("realm1",
                                             new WebSocket4NetTextConnection<JToken>(serverAddress,
                                                                                     jTokenBinding),
                                             jTokenBinding);

            wampChannel.Open().Wait();

            ISubject<int> subject =
                wampChannel.RealmProxy.Services.GetSubject<int>("com.myapp.topic1");

            IDisposable disposable = subject.Subscribe(x => GetValue(x));
            return disposable;
        }

        private static IDisposable ServerCode(WampHost host)
        {
            IWampRealm realm = host.RealmContainer.GetRealmByName("realm1");

            IWampTopicContainer topicContainer = realm.TopicContainer;

            IWampTopic topic =
                topicContainer.GetOrCreateTopicByUri("com.myapp.topic1", true);

            IDisposable disposable = realm.Services.GetSubject<int>("com.myapp.topic1")
                                          .Subscribe(x => GetValue(x));

            return disposable;
        }

        private static void GetValue(int number)
        {
            Console.WriteLine("Got " + number);
        }
    }
}