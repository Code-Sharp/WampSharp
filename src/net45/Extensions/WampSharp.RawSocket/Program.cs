using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MsgPack;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Client;
using WampSharp.V2.Rpc;

namespace WampSharp.RawSocket
{
    public interface ITimeService
    {
        [WampProcedure("com.timeservice.now")]
        Task<string> UtcNow();
    }

    class Program
    {

        public class TimeService
        {
            [WampProcedure("com.timeservice.now")]
            public string UtcNow()
            {
                DateTime date = DateTime.UtcNow;
                return date.ToString("yyyy-MM-ddTHH:mm:ssK");
            }
        }

        static void Main(string[] args)
        {
            WampHost myHost = new WampHost();

            myHost.RealmContainer.GetRealmByName("realm1").Services.RegisterCallee
                (new TimeService());

            var binding = new JTokenJsonBinding();

            IWampTransport transport = new RawSocketTransport(new TcpListener(IPAddress.Any, 8080));

            myHost.RegisterTransport(transport,
                                     binding);

            myHost.Open();

            MyCode(new JTokenMsgpackBinding());

            Console.ReadLine();

            myHost.Dispose();

            myHost = new WampHost();

            myHost.RealmContainer.GetRealmByName("realm1").Services.RegisterCallee
                (new TimeService());

            transport = new RawSocketTransport(new TcpListener(IPAddress.Any, 8080));

            myHost.RegisterTransport(transport,
                                     new JTokenMsgpackBinding());

            myHost.Open();

            Console.ReadLine();
        }

        private static void MyCode(JTokenMsgpackBinding binding)
        {
            WampChannelFactory factory = new WampChannelFactory();

            IWampChannel wampChannel = factory.CreateChannel
                ("realm1",
                 new RawSocketClientConnection<JToken>(() => new TcpClient(),
                                                       IPAddress.Parse("127.0.0.1"), 8080,
                                                       binding),
                 binding);


            WampChannelReconnector reconnector =
                new WampChannelReconnector
                    (wampChannel,
                     async () =>
                     {
                         Console.WriteLine("Connecting");

                         await wampChannel.Open().ConfigureAwait(false);

                         ITimeService calleeProxy =
                             wampChannel.RealmProxy.Services.GetCalleeProxy<ITimeService>();

                         string now = await calleeProxy.UtcNow();

                         Console.WriteLine(now);
                     });

            reconnector.Start();
        }

        private static async Task MyCode2(JTokenJsonBinding binding)
        {
            WampChannelFactory factory = new WampChannelFactory();

            IWampChannel wampChannel = factory.CreateChannel
                ("realm1",
                 new RawSocketClientConnection<JToken>(() => new TcpClient(),
                                                       IPAddress.Parse("127.0.0.1"), 8080,
                                                       binding),
                 binding);

            await wampChannel.Open();

            ITimeService calleeProxy = 
                wampChannel.RealmProxy.Services.GetCalleeProxy<ITimeService>();

            string now = await calleeProxy.UtcNow();
        }
    }
}