using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WampSharp.Api;
using WampSharp.Rpc;

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
            WampChannelFactory channelFactory = new WampChannelFactory();

            IWampChannel<JToken> channel =
                channelFactory.CreateChannel("ws://localhost:9000/");

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