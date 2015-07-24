using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Rpc;

namespace WampSharp.RawSocket
{
    public enum FrameType
    {
        WampMessage = 0,
        Ping = 1,
        Pong = 2
    }

    public enum SerializerType
    {
        Illegal = 0,
        Json = 1,
        MsgPack = 2
    }

    public class TimeService
    {
        [WampProcedure("com.timeservice.now")]
        public string UtcNow()
        {
            DateTime date = DateTime.UtcNow;
            return date.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            WampHost myHost = new WampHost();

            myHost.RealmContainer.GetRealmByName("realm1").Services.RegisterCallee(new TimeService());

            myHost.RegisterTransport(new TcpListenerRawSocketTransport(IPAddress.Any, 8080),
                                     new JTokenJsonBinding());

            myHost.Open();

            Console.ReadLine();
        }
    }
}
