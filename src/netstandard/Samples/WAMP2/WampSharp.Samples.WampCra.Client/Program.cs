using System;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Fluent;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.WampCra.Client
{
    public class Program
    {
        static async Task Main(string[] arguments)
        {
            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            IWampChannel channel =
                channelFactory.ConnectToRealm("realm1")
                              .WebSocketTransport(new Uri("ws://127.0.0.1:8080/ws"))
                              .JsonSerialization()
                              .CraAuthentication("peter", "secret1")
                              //.CraAuthentication("joe", "secret2")
                              .Build();

            channel.RealmProxy.Monitor.ConnectionEstablished +=
                (sender, args) =>
                {
                    Console.WriteLine("connected session with ID " + args.SessionId);

                    WelcomeDetails details = args.WelcomeDetails;

                    Console.WriteLine($"authenticated using method '{details.AuthenticationMethod}' and provider '{details.AuthenticationProvider}'");

                    Console.WriteLine($"authenticated with authid '{details.AuthenticationId}' and authrole '{details.AuthenticationRole}'");
                };

            channel.RealmProxy.Monitor.ConnectionBroken += (sender, args) =>
                                                           {
                                                               GoodbyeAbortDetails details = args.Details;
                                                               Console.WriteLine($"disconnected {args.Reason} {details.Message}");
                                                           };

            IWampRealmProxy realmProxy = channel.RealmProxy;

            await channel.Open().ConfigureAwait(false);

            // call a procedure we are allowed to call (so this should succeed)
            //
            IAdd2Service proxy = realmProxy.Services.GetCalleeProxy<IAdd2Service>();

            try
            {
                var five = await proxy.Add2Async(2, 3);
                Console.WriteLine("call result {0}", five);
            }
            catch (Exception e)
            {
                Console.WriteLine("call error {0}", e);
            }

            // (try to) register a procedure where we are not allowed to (so this should fail)
            //
            Mul2Service service = new Mul2Service();

            try
            {
                await realmProxy.Services.RegisterCallee(service)
                                .ConfigureAwait(false);

                Console.WriteLine("huh, function registered!");
            }
            catch (WampException ex)
            {
                Console.WriteLine("registration failed - this is expected: " + ex.ErrorUri);
            }

            // (try to) publish to some topics
            //
            string[] topics =
            {
                "com.example.topic1",
                "com.example.topic2",
                "com.foobar.topic1",
                "com.foobar.topic2"
            };

            foreach (string topic in topics)
            {
                IWampTopicProxy topicProxy = realmProxy.TopicContainer.GetTopicByUri(topic);

                try
                {
                    await topicProxy.Publish(new PublishOptions() {Acknowledge = true},
                                             new object[] {"hello"})
                                    .ConfigureAwait(false);

                    Console.WriteLine("event published to topic " + topic);
                }
                catch (WampException ex)
                {
                    Console.WriteLine("publication to topic " + topic + " failed: " + ex.ErrorUri);
                }
            }

            Console.ReadLine();
        }

        public interface IAdd2Service
        {
            [WampProcedure("com.example.add2")]
            Task<int> Add2Async(int x, int y);
        }

        public class Mul2Service
        {
            [WampProcedure("com.example.mul2")]
            public int Multiply2(int x, int y)
            {
                return x * y;
            }
        }
    }
}