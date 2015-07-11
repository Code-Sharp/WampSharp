using System.Security.Cryptography.X509Certificates;
using WampSharp.V2.Authentication;

namespace WampSharp.Fleck
{
    public class FleckAuthenticatedWebSocketTransport : FleckWebSocketTransport
    {
        public FleckAuthenticatedWebSocketTransport
            (string location,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null)
            : base(location, cookieAuthenticatorFactory, certificate)
        {
        }
    }
}