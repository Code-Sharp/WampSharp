using System.Net;
using System.Security.Cryptography.X509Certificates;
using WampSharp.V2.Authentication;

namespace WampSharp.Vtortola
{
    public class VtortolaAuthenticatedWebSocketTransport : VtortolaWebSocketTransport
    {
        public VtortolaAuthenticatedWebSocketTransport
            (IPEndPoint endpoint,
             bool perMessageDeflate,
             ICookieAuthenticatorFactory authenticatorFactory,
             X509Certificate2 certificate = null)
            : base(endpoint, perMessageDeflate, authenticatorFactory, certificate)
        {
        }
    }
}