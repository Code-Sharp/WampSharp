using System.Net.Security;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    internal static class SslStreamExtensions
    {
        public static Task AuthenticateAsClientAsync(this SslStream stream, ClientSslConfiguration configuration)
        {
            return stream.AuthenticateAsClientAsync(configuration.TargetHost,
                configuration.ClientCertificates,
                configuration.EnabledSslProtocols,
                configuration.CheckCertificateRevocation);
        }

        public static Task AuthenticateAsServerAsync(this SslStream stream, ServerSslConfiguration configuration)
        {
            return stream.AuthenticateAsServerAsync(configuration.Certificate,
                configuration.ClientCertificateRequired,
                configuration.EnabledSslProtocols,
                configuration.CheckCertificateRevocation);
        }
    }
}