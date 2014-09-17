using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.V2;

namespace WampSharp.Samples.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverAddress = "ws://127.0.0.1:8080/ws";

            WampHost host = new DefaultWampHost(serverAddress);

            host.Open();

            IDisposable disposable = ClientCode(serverAddress);

            Console.ReadLine();

            disposable.Dispose();

            Console.ReadLine();
        }

        private static IDisposable ClientCode(string serverAddress)
        {
            DefaultWampChannelFactory channelFactory =
                new DefaultWampChannelFactory();

            IWampChannel wampChannel =
                channelFactory.CreateJsonChannel(serverAddress, "realm1");

            wampChannel.Open().Wait();

            ISubject<int> subject =
                wampChannel.RealmProxy.Services.GetSubject<int>("com.myapp.topic1");

            int counter = 0;

            IObservable<long> timer =
                Observable.Timer(TimeSpan.FromMilliseconds(0),
                                 TimeSpan.FromMilliseconds(1000));

            IDisposable disposable = 
                timer.Subscribe(x =>
                {
                    counter++;
                    Console.WriteLine("Publishing to topic 'com.myapp.topic1': " + counter);
                    subject.OnNext(counter);
                });

            return disposable;
        }
    }
}