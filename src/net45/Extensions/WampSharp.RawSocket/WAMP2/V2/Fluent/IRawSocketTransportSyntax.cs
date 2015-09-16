using System;
using System.Net;
using System.Net.Sockets;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        IRawSocketTransportConnectFromSyntax ConnectFrom(AddressFamily family);
        IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint);
        IRawSocketTransportConnectFromSyntax ConnectFrom(string hostname, int port);

        ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan interval);
    }
}