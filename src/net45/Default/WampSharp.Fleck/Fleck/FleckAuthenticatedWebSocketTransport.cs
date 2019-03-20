using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WampSharp.V2.Authentication;

namespace WampSharp.Fleck
{
    public class FleckAuthenticatedWebSocketTransport : FleckWebSocketTransport
    {
        public FleckAuthenticatedWebSocketTransport
            (string location,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null,
             bool supportDualStack = true)
            : this(location: location, supportDualStack: supportDualStack, cookieAuthenticatorFactory: cookieAuthenticatorFactory, certificate: certificate, getEnabledSslProtocols: null)
        {
        }

        public FleckAuthenticatedWebSocketTransport
            (string location,
             ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
             X509Certificate2 certificate = null,
             Func<SslProtocols> getEnabledSslProtocols = null,
             bool supportDualStack = true)
            : base(location, cookieAuthenticatorFactory, certificate, getEnabledSslProtocols, supportDualStack)
        {
        }
    }
}