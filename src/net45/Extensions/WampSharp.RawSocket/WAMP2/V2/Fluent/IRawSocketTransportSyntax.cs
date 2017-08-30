using System;
using System.Net;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint);
        ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan interval);
    }
}