using System;
using System.Threading.Tasks;
using CliFx.Attributes;
using Newtonsoft.Json;
using WampSharp.Samples.Common;
using WampSharp.V2;

namespace WampSharp.Samples.Subscriber.Rx
{
    [Command("complex rx")]
    public class ComplexCommand : SampleCommand 
    {
        protected override async Task RunAsync(IWampChannel channel)
        {
            await channel.Open().ConfigureAwait(false);

            await Task.Yield();
  
            IWampSubject heartbeatSubject =
                channel.RealmProxy.Services.GetSubject("com.myapp.heartbeat");

            var topic2Subject =
                channel.RealmProxy.Services.GetSubject("com.myapp.topic2", new Topic2TupleConverter());

            using (IDisposable heartbeatDisposable =
                heartbeatSubject.Subscribe(value => Console.WriteLine($"Got heartbeat (publication ID {value.PublicationId})"),
                                ex => Console.WriteLine($"An error has occurred {ex}")))
            {
                void Topic2EventHandler((int number1, int number2, string c, MyClass d) value)
                {
                    (int number1, int number2, string c, MyClass d) = value;
                    Console.WriteLine($@"Got event: number1:{number1}, number2:{number2}, c:""{c}"", d:{{{d}}}");
                }

                void Topic2ErrorHandler(Exception exception)
                {
                    Console.WriteLine($"An error has occurred {exception}");
                }

                using (IDisposable topic2Disposable =
                    topic2Subject.Subscribe(Topic2EventHandler, Topic2ErrorHandler))
                {
                    Console.WriteLine("Hit enter to unsubscribe");

                    Console.ReadLine();
                }
            }

            Console.WriteLine("Unsubscribed");

            Console.ReadLine();
        }
    }

    internal class Topic2TupleConverter : 
        WampEventValueTupleConverter<(int number1, int number2, string c, MyClass d)>
    {
    }

    public class MyClass
    {
        [JsonProperty("counter")]
        public int Counter { get; set; }

        [JsonProperty("foo")]
        public int[] Foo { get; set; }

        public override string ToString()
        {
            return $"counter: {Counter}, foo: [{string.Join(", ", Foo)}]";
        }
    }
}