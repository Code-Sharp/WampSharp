using System;
using System.Net;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
#if !NETCORE
        IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint);
#endif

        ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan interval);
    }
}