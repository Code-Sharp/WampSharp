using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WampSharp.RawSocket
{
    public class SslConfiguration
    {
        public SslProtocols EnabledSslProtocols { get; set; }
        public bool CheckCertificateRevocation { get; set; }
    }

    public class ClientSslConfiguration : SslConfiguration
    {
        public ClientSslConfiguration(string targetHost, X509CertificateCollection clientCertificates, SslProtocols enabledSslProtocols, bool checkCertificateRevocation)
        {
            TargetHost = targetHost;
            ClientCertificates = clientCertificates;
            EnabledSslProtocols = enabledSslProtocols;
            CheckCertificateRevocation = checkCertificateRevocation;
        }

        public string TargetHost { get; set; }
        public X509CertificateCollection ClientCertificates { get; set; }
    }

    public class ServerSslConfiguration : SslConfiguration
    {
        public bool ClientCertificateRequired { get; set; }
        public X509Certificate Certificate { get; set; }
    }
}