using System.Security.Authentication;

namespace WampSharp.RawSocket
{
    public class SslConfiguration
    {
#if NETSTANDARD1_4
        protected const SslProtocols DefaultSslProtocols = SslProtocols.Tls | SslProtocols.Ssl3;
#else
        protected const SslProtocols DefaultSslProtocols = SslProtocols.Default;
#endif

        public SslProtocols EnabledSslProtocols { get; set; }
        public bool CheckCertificateRevocation { get; set; }
    }
}