using System;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WampSharp.RawSocket;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportSyntax : IRawSocketTransportConnectFromSyntax
    {
#if !NETCORE
        IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint);
#endif
    }
}