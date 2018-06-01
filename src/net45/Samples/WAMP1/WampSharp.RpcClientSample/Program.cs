using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WampSharp.V1;
using WampSharp.V1.Rpc;

namespace WampSharp.RpcClientSample
{
    public class Program
    {
        public interface ICalculator
        {
            [WampRpcMethod("http://example.com/simple/calc#add")]
            int Add(int x, int y);
        }

        public interface IAsyncCalculator
        {
            [WampRpcMethod("http://example.com/simple/calc#add")]
            Task<int> Add(int x, int y);
        }

        static void Main(string[] args)
        {
            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            IWampChannel<JToken> channel =
                channelFactory.CreateChannel("ws://127.0.0.1:9000/");

            channel.Open();

            ICalculator proxy = channel.GetRpcProxy<ICalculator>();

            int five = proxy.Add(2, 3);

            Console.WriteLine("2 + 3 = " + five);

            IAsyncCalculator asyncProxy =
                channel.GetRpcProxy<IAsyncCalculator>();

            Task<int> asyncFive =
                asyncProxy.Add(2, 3);

            Console.WriteLine("2 + 3 = " + asyncFive.Result);

            Console.ReadLine();
        }
    }
}