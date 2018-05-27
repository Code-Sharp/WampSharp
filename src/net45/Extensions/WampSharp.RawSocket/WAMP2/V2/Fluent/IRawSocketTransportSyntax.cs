using System.Net;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : IRawSocketTransportConnectFromSyntax
    {
#if !NETCORE
        IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint);
#endif
    }
}