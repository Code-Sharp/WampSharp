using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using WampSharp.Samples.Common;
using WampSharp.V2;
using WampSharp.V2.PubSub;

namespace WampSharp.Samples.Publisher.Reflection
{
    [Command("basic reflection")]
    public class BasicCommand : SampleCommand
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            BasicPublisher publisher = new BasicPublisher();

            using (IDisposable disposable =
                channel.RealmProxy.Services.RegisterPublisher(publisher))
            {
                Console.WriteLine("Hit enter to stop publishing");

                Console.ReadLine();
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }        
    }

    public interface IBasicPublisher
    {
        [WampTopic("com.myapp.topic1")]
        event Action<int> CounterUpdated;
    }

    public class BasicPublisher : IBasicPublisher, IDisposable
    {
        private IDisposable mSubscription;
        public event Action<int> CounterUpdated;

        public BasicPublisher()
        {
            mSubscription =
                Observable.Timer(TimeSpan.FromSeconds(0),
                                 TimeSpan.FromSeconds(1)).Select((x, i) => i)
                          .Subscribe(x => OnTimer(x));
        }

        private void OnTimer(int counter)
        {
            CounterUpdated?.Invoke(counter);
        }

        public void Dispose()
        {
            mSubscription?.Dispose();
            mSubscription = null;
        }
    }
}