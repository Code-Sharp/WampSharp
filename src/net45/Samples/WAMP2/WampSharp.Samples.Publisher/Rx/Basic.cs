using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2;

namespace WampSharp.Samples.Publisher.Rx
{
    class BasicProgram
    {
        public static async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();

            ISubject<int> topic = channel.RealmProxy.Services.GetSubject<int>("com.myapp.topic1");

            IObservable<int> timer =
                Observable.Timer(TimeSpan.FromSeconds(0),
                                 TimeSpan.FromSeconds(1)).Select((x, i) => i);

            using (IDisposable disposable = timer.Subscribe(topic))
            {
                Console.WriteLine("Hit enter to stop publishing");

                Console.ReadLine();
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }
    }
}