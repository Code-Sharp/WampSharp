using System.Net;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : IRawSocketTransportConnectFromSyntax
    {
        IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint);
    }
}