using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Samples.Publisher.Raw
{
    class OptionsProgram
    {
        public static async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();

            IWampTopicProxy topic = 
                channel.RealmProxy.TopicContainer.GetTopicByUri("com.myapp.topic1");

            IObservable<int> timer =
                Observable.Timer(TimeSpan.FromSeconds(0),
                                 TimeSpan.FromSeconds(1)).Select((x, i) => i);

            PublishOptions publishOptions = new PublishOptions(){DiscloseMe = true, Acknowledge = true};

            async Task<(int value, long? publicationId)> TopicSelector(int value)
            {
                long? publicationId = await topic.Publish(publishOptions, new object[] {value});
                return (value, publicationId);
            }

            void SubscriptionHandler((int value, long? publicationId) valueTuple)
            {
                (int value, long? publicationId) = valueTuple;
                Console.WriteLine($"Published with publication ID {publicationId}");
            }

            void ErrorHandler(Exception exception)
            {
                Console.WriteLine("An error has occurred: " + exception);
            }

            using (IDisposable disposable = timer.SelectMany(value => TopicSelector(value))
                                                 .Subscribe(SubscriptionHandler, ex => ErrorHandler(ex)))
            {
                Console.WriteLine("Hit enter to stop publishing");

                Console.ReadLine();
            }

            Console.WriteLine("Stopped publishing");

            Console.ReadLine();
        }
    }
}