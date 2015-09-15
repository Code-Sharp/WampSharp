using System.Net;
using System.Net.Sockets;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        ChannelFactorySyntax.ITransportSyntax ConnectFrom(AddressFamily family);
        ChannelFactorySyntax.ITransportSyntax ConnectFrom(IPEndPoint localEndPoint);
        ChannelFactorySyntax.ITransportSyntax ConnectFrom(string hostname, int port);
    }
}