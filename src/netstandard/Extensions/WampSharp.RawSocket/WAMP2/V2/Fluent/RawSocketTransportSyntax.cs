using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WampSharp.RawSocket;
using static WampSharp.RawSocket.SslConfiguration;

namespace WampSharp.V2.Fluent
{
    internal class RawSocketTransportSyntax : IRawSocketTransportSyntax,
        IRawSocketTransportConnectFromSyntax
    {
        public RawSocketTransportSyntax(ChannelState state)
        {
            State = state;
        }

        public ChannelState State { get; set; }

        public IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint)
        {
            return InnerConnectFrom(() => new TcpClient(localEndPoint));
        }

        private RawSocketTransportSyntax InnerConnectFrom(Func<TcpClient> clientBuilder)
        {
            ((RawSocketActivator) State.ConnectionActivator).ClientBuilder = clientBuilder;
            return this;
        }

        public ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan autoPingInterval)
        {
            return InnerSetInterval(autoPingInterval);
        }

        public IRawSocketTransportSyntax SslConfiguration(ClientSslConfiguration sslConfiguration)
        {
            ((RawSocketActivator)State.ConnectionActivator).SslConfiguration = sslConfiguration;
            return this;
        }

        public IRawSocketTransportSyntax SslConfiguration(string targetHost, X509CertificateCollection clientCertificates = null, SslProtocols enabledSslProtocols = DefaultSslProtocols, bool checkCertificateRevocation = false)
        {
            return SslConfiguration(new ClientSslConfiguration(targetHost, clientCertificates, enabledSslProtocols,
                checkCertificateRevocation));
        }

        private ChannelState InnerSetInterval(TimeSpan autoPingInterval)
        {
            ((RawSocketActivator)State.ConnectionActivator).AutoPingInterval = autoPingInterval;
            return State;
        }
    }
}