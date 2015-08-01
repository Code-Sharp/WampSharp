using System;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.WampCra.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
#if NET45
            Task clientTask = ClientCode();
            clientTask.Wait();
#else
            ClientCode();
#endif
        }

#if NET45
        public static async Task ClientCode()
#else
        public static void ClientCode()
#endif
        {
            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            IWampClientAuthenticator authenticator;

            if (false)
            {
                authenticator = new WampCraClientAuthenticator(authenticationId: "joe", secret: "secret2");
            }
            else
            {
                authenticator =
                    new WampCraClientAuthenticator(authenticationId: "peter", secret: "secret1", salt: "salt123", iterations: 100, keyLen: 16);
            }

            IWampChannel channel =
                channelFactory.CreateJsonChannel("ws://127.0.0.1:8080/ws",
                    "realm1",
                    authenticator);

            channel.RealmProxy.Monitor.ConnectionEstablished +=
                (sender, args) =>
                {
                    Console.WriteLine("connected session with ID " + args.SessionId);

                    dynamic details = args.WelcomeDetails.OriginalValue.Deserialize<dynamic>();

                    Console.WriteLine("authenticated using method '{0}' and provider '{1}'", details.authmethod,
                                      details.authprovider);

                    Console.WriteLine("authenticated with authid '{0}' and authrole '{1}'", details.authid,
                                      details.authrole);
                };

            channel.RealmProxy.Monitor.ConnectionBroken += (sender, args) =>
            {
                dynamic details = args.Details.OriginalValue.Deserialize<dynamic>();
                Console.WriteLine("disconnected " + args.Reason + " " + details.reason + details);
            };

            IWampRealmProxy realmProxy = channel.RealmProxy;

#if NET45
            await channel.Open().ConfigureAwait(false);
#else
            channel.Open().Wait();
#endif
            // call a procedure we are allowed to call (so this should succeed)
            //
            IAdd2Service proxy = realmProxy.Services.GetCalleeProxy<IAdd2Service>();

            try
            {
#if NET45
                var five = await proxy.Add2Async(2, 3)
                    .ConfigureAwait(false);
#else
                var five = proxy.Add2Async(2, 3).Result;
#endif
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
#if NET45
                await realmProxy.Services.RegisterCallee(service)
                    .ConfigureAwait(false);
#else
                realmProxy.Services.RegisterCallee(service)
                    .Wait();
#endif

                Console.WriteLine("huh, function registered!");
            }
#if NET45
            catch (WampException ex)
            {
                Console.WriteLine("registration failed - this is expected: " + ex.ErrorUri);
            }
#else
            catch (AggregateException ex)
            {
                WampException innerException = ex.InnerException as WampException;
                Console.WriteLine("registration failed - this is expected: " + innerException.ErrorUri);
            }
#endif

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
#if NET45
                    await topicProxy.Publish(new PublishOptions() { Acknowledge = true },
                        new object[] { "hello" })
                        .ConfigureAwait(false);
#else
                    topicProxy.Publish(new PublishOptions() { Acknowledge = true },
                        new object[] { "hello" })
                        .Wait();
#endif
                    Console.WriteLine("event published to topic " + topic);
                }
#if NET45
                catch (WampException ex)
                {
                    Console.WriteLine("publication to topic " + topic + " failed: " + ex.ErrorUri);
                }
#else
                catch (AggregateException ex)
                {
                    WampException innerException = ex.InnerException as WampException;
                    Console.WriteLine("publication to topic " + topic + " failed: " + innerException.ErrorUri);
                }
#endif
            }
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