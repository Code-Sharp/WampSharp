using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Binding;
using WampSharp.Samples.Caller.Contracts;
using WampSharp.V2;
using WampSharp.V2.Realm;

namespace WampSharp.Samples.Caller
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: WampSharp.Samples.Caller.exe sample [client|router]");
                Console.WriteLine("Where sample is one of the following values:");

                foreach (string name in
                    typeof (Program).GetMethods(BindingFlags.NonPublic |
                                                BindingFlags.Static)
                                    .Where(x => x.IsDefined(typeof (SampleAttribute), true))
                                    .Select(x => x.GetCustomAttribute<SampleAttribute>().Name))
                {
                    Console.WriteLine(" {0}", name);
                }

                args = new string[] {"arguments", "router"};
            }

            string sampleName = args[0];
            string routerOrClient = args[1];

            bool router = StringComparer.InvariantCultureIgnoreCase.Equals(routerOrClient, "router");

            MethodInfo sampleMethod = GetSampleMethod(sampleName);

            string serverAddress = "ws://127.0.0.1:8080/ws";

            if (router)
            {
                RouterCode(serverAddress, sampleMethod);
            }
            else
            {
                ClientCode(serverAddress, sampleMethod);
            }
        }

        private static void RouterCode(string serverAddress, MethodInfo sampleMethod)
        {
            DefaultWampHost host = new DefaultWampHost(serverAddress);

            IWampRealm realm = host.RealmContainer.GetRealmByName("realm1");

            host.Open();

            Console.WriteLine("Server is up");

            Console.WriteLine("Press any key to start demo.");

            Console.ReadLine();

            sampleMethod.Invoke(null, new object[] { realm.Services });

            Console.ReadLine();
        }

        private static void ClientCode(string serverAddress, MethodInfo sampleMethod)
        {
            JTokenBinding binding = new JTokenBinding();

            DefaultWampChannelFactory factory =
                new DefaultWampChannelFactory();

            IWampChannel channel =
                factory.CreateChannel(serverAddress, "realm1", binding);

            Task task = channel.Open();
            task.Wait(5000);

            if (!task.IsCompleted)
            {
                Console.WriteLine("Server might be down.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Connected to server.");
            }

            sampleMethod.Invoke(null, new object[] { channel.RealmProxy.Services });

            Console.ReadLine();
        }

        private static MethodInfo GetSampleMethod(string sampleName)
        {
            return typeof (Program).GetMethods(BindingFlags.NonPublic |
                                               BindingFlags.Static)
                                   .Where(x => x.IsDefined(typeof (SampleAttribute), true))
                                   .FirstOrDefault(x =>StringComparer.InvariantCultureIgnoreCase.Equals(x.GetCustomAttribute<SampleAttribute>().Name, sampleName));
        }

        [Sample("arguments")]
        private static void Arguments(IWampRealmServiceProvider serviceProvider)
        {
            IArgumentsService proxy = serviceProvider.GetCalleeProxy<IArgumentsService>();

            proxy.Ping();
            Console.WriteLine("Pinged!");

            int result = proxy.Add2(2, 3);
            Console.WriteLine("Add2: {0}", result);

            var starred = proxy.Stars();
            Console.WriteLine("Starred 1: {0}", starred);

            starred = proxy.Stars(nick: "Homer");
            Console.WriteLine("Starred 2: {0}", starred);

            starred = proxy.Stars(stars: 5);
            Console.WriteLine("Starred 3: {0}", starred);

            starred = proxy.Stars(nick: "Homer", stars: 5);
            Console.WriteLine("Starred 4: {0}", starred);

            string[] orders = proxy.Orders("coffee");
            Console.WriteLine("Orders 1: {0}", string.Join(", ", orders));

            orders = proxy.Orders("coffee", limit: 10);
            Console.WriteLine("Orders 2: {0}", string.Join(", ", orders));
        }
    }
}