using System;
using WampSharp.V1;
using WampSharp.V1.Rpc;

namespace WampSharp.RpcServerSample
{
    public interface ICalculator
    {
        [WampRpcMethod("http://example.com/simple/calc#add")]
        int Add(int x, int y);
    }

    internal class Calculator : ICalculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            // http://autobahn.ws/static/file/autobahnjs.html

            const string location = "ws://127.0.0.1:9000/";
            using (IWampHost host = new DefaultWampHost(location))
            {
                ICalculator instance = new Calculator();
                host.HostService(instance);

                host.Open();

                Console.WriteLine("Server is running on " + location);
                Console.ReadLine();
            }
        }
    }
}