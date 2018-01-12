using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WampSharp.RawSocket;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportConnectFromSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan interval);

        IRawSocketTransportSyntax SslConfiguration(ClientSslConfiguration sslConfiguration);

        IRawSocketTransportSyntax SslConfiguration(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation);
    }
}