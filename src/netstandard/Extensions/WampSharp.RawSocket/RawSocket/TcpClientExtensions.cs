#if NETCORE
using System.Net.Sockets;

namespace WampSharp.RawSocket
{
    internal static class TcpClientExtensions
    {
        public static void Close(this TcpClient client)
        {
            client.Dispose();            
        }
    }
}

#endif