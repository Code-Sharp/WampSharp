using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.MetaApi;

namespace WampSharp.Samples.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serverAddress = "ws://127.0.0.1:8080/ws";

            ClientCode(serverAddress);

            Console.ReadLine();
        }

        private static IDisposable ClientCode(string serverAddress)
        {
            DefaultWampChannelFactory channelFactory =
                new DefaultWampChannelFactory();

            IWampChannel wampChannel =
                channelFactory.CreateJsonChannel(serverAddress, "realm1");

            wampChannel.Open().Wait();

            IWampSubject subject =
                wampChannel.RealmProxy.Services.GetSubject("com.myapp.topic1");

            int counter = 0;

            IObservable<long> timer =
                Observable.Timer(TimeSpan.FromMilliseconds(0),
                                 TimeSpan.FromMilliseconds(1000));

            IDisposable disposable =
                timer.Subscribe(x =>
                {
                    counter++;
                    Console.WriteLine("Publishing to topic 'wamp.myapp.counter': " + counter);
                    subject.OnNext(new WampEvent()
                    {
                        Arguments = new object[]{counter },
                        Options = new PublishOptions() { Retain = true}
                    });
                });

            return disposable;
        }
    }
}