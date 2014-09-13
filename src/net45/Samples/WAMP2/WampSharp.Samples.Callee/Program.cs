using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;
using WampSharp.WebSocket4Net;

namespace WampSharp.Samples.Callee
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage: WampSharp.Samples.Callee.exe sample [client|router]");
                Console.WriteLine("Where sample is one of the following values:");

                foreach (string name in
                    typeof(SampleAttribute).Assembly
                            .GetExportedTypes()
                            .Where(x => x.IsDefined(typeof (SampleAttribute), true))
                            .Select(x => x.GetCustomAttribute<SampleAttribute>().Name))
                {
                    Console.WriteLine(" {0}", name);
                }

                args = new string[] {"timeservice", "router"};
            }

            string sampleName = args[0];
            string routerOrClient = args[1];

            bool router = StringComparer.InvariantCultureIgnoreCase.Equals(routerOrClient, "router");

            Type sampleType = GetSampleType(sampleName);

            IEnumerable<IWampRpcOperation> operations = 
                GetSampleOperations(sampleType);

            string serverAddress = "ws://127.0.0.1:8080/ws";

            if (router)
            {
                RouterCode(serverAddress, operations);
            }
            else
            {
                ClientCode(serverAddress, operations);
            }
        }

        private static void RouterCode(string serverAddress, IEnumerable<IWampRpcOperation> operations)
        {
            DefaultWampHost host = new DefaultWampHost(serverAddress);

            IWampRealm realm = host.RealmContainer.GetRealmByName("realm1");

            foreach (IWampRpcOperation operation in operations)
            {
                realm.RpcCatalog.Register(operation);
            }

            host.Open();
            Console.WriteLine("Server is up");
            Console.ReadLine();
        }

        private static void ClientCode(string serverAddress, IEnumerable<IWampRpcOperation> operations)
        {
            MessagePackObjectBinding binding = new MessagePackObjectBinding();

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

            var dummy = new RegisterOptions();

            var tasks = new List<Task>();

            foreach (IWampRpcOperation operation in operations)
            {
                Task registrationTask = channel.RealmProxy.RpcCatalog.Register(operation, dummy);
                tasks.Add(registrationTask);
            }

            bool complete = Task.WaitAll(tasks.ToArray(), TimeSpan.FromSeconds(30));

            if (complete)
            {
                Console.WriteLine("All operations registered.");
            }

            Console.ReadLine();
        }

        private static IEnumerable<IWampRpcOperation> GetSampleOperations(Type sampleType)
        {
            object instance = Activator.CreateInstance(sampleType);

            foreach (MethodInfo method in
                sampleType.GetMethods()
                          .Where(x => x.IsDefined(typeof (WampProcedureAttribute), true)))
            {
                yield return new SyncMethodInfoRpcOperation(instance, method);
            }

            foreach (Type nestedType in
                sampleType.GetNestedTypes(BindingFlags.NonPublic |
                                          BindingFlags.Public)
                          .Where(x => typeof (IWampRpcOperation).IsAssignableFrom(x)))
            {
                yield return (IWampRpcOperation) Activator.CreateInstance(nestedType);
            }
        }

        private static Type GetSampleType(string sampleName)
        {
            return typeof(SampleAttribute).Assembly.GetExportedTypes()
                           .Where(x => x.IsDefined(typeof(SampleAttribute), true))
                           .FirstOrDefault(x => StringComparer.InvariantCultureIgnoreCase.Equals(x.GetCustomAttribute<SampleAttribute>().Name, sampleName));
        }
    }
}