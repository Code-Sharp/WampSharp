using System.Net;
using System.Security.Cryptography.X509Certificates;
using vtortola.WebSockets;
using WampSharp.V2.Authentication;

namespace WampSharp.Vtortola
{
    public class VtortolaAuthenticatedWebSocketTransport : VtortolaWebSocketTransport
    {
        public VtortolaAuthenticatedWebSocketTransport
            (IPEndPoint endpoint, bool perMessageDeflate, ICookieAuthenticatorFactory authenticatorFactory, WebSocketListenerOptions options)
            : this(endpoint, perMessageDeflate, authenticatorFactory, null, options)
        {
        }

        public VtortolaAuthenticatedWebSocketTransport
            (IPEndPoint endpoint, bool perMessageDeflate, ICookieAuthenticatorFactory authenticatorFactory, X509Certificate2 certificate = null, WebSocketListenerOptions options = null)
            : base(endpoint, perMessageDeflate, authenticatorFactory, certificate, options)
        {
        }
    }
}