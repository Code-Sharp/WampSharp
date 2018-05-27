using System.Security.Authentication;

namespace WampSharp.RawSocket
{
    public class SslConfiguration
    {
        public SslProtocols EnabledSslProtocols { get; set; }
        public bool CheckCertificateRevocation { get; set; }
    }
}