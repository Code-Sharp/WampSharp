using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WampSharp.RawSocket
{
    public class ServerSslConfiguration : SslConfiguration
    {
        public ServerSslConfiguration(X509Certificate serverCertificate,
                                      bool clientCertificateRequired = false,
                                      SslProtocols enabledSslProtocols = DefaultSslProtocols,
                                      bool checkCertificateRevocation = false)
        {
            Certificate = serverCertificate;
            ClientCertificateRequired = clientCertificateRequired;
            EnabledSslProtocols = enabledSslProtocols;
            CheckCertificateRevocation = checkCertificateRevocation;
        }

        public bool ClientCertificateRequired { get; set; }
        public X509Certificate Certificate { get; }
    }
}