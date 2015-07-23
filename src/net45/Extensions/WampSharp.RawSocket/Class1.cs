using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WampSharp.Binding;
using WampSharp.V2;

namespace WampSharp.RawSocket
{
    public enum MessageType
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

    class Program
    {
        static void Main(string[] args)
        {
            WampHost myHost = new WampHost();

            myHost.RegisterTransport(new TcpListenerRawSocketTransport(IPAddress.Any, 8080),
                                     new JTokenJsonBinding());

            myHost.Open();

            Console.ReadLine();
        }

        static void Main2(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            
            listener.Start();

            TcpClient tcpClient = listener.AcceptTcpClient();

            byte[] buffer = new byte[4];
            int readBytes = tcpClient.GetStream().Read(buffer, 0, 4);

            Handshake request = new Handshake(buffer);

            Handshake response = request.GetAcceptedResponse(15);

            byte[] responseArray = response.ToArray();

            tcpClient.GetStream().Write(responseArray, 0, responseArray.Length);

            readBytes = tcpClient.GetStream().Read(buffer, 0, 4);

            WampFrameHeader header = new WampFrameHeader(buffer);

            byte[] array = new byte[header.MessageLength];
            readBytes = tcpClient.GetStream().Read(array, 0, array.Length);

            string message = Encoding.UTF8.GetString(array);
        }
    }
}
