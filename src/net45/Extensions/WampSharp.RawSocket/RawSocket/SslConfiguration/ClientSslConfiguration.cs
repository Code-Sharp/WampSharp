using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace WampSharp.RawSocket
{
    public class ClientSslConfiguration : SslConfiguration
    {
        public ClientSslConfiguration(string targetHost, 
                                      X509CertificateCollection clientCertificates = null, 
                                      SslProtocols enabledSslProtocols = DefaultSslProtocols, 
                                      bool checkCertificateRevocation = false)
        {
            TargetHost = targetHost;
            ClientCertificates = clientCertificates ?? new X509CertificateCollection();
            EnabledSslProtocols = enabledSslProtocols;
            CheckCertificateRevocation = checkCertificateRevocation;
        }

        public string TargetHost { get; }

        public X509CertificateCollection ClientCertificates { get; set; }
    }
}