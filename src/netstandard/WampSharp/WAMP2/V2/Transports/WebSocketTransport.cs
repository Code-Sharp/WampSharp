using WampSharp.V2.Authentication;

namespace WampSharp.V2.Transports
{
    /// <summary>
    /// Represents a WebSocket transport.
    /// </summary>
    public abstract class WebSocketTransport<TConnection> : TextBinaryTransport<TConnection>
    {
        public WebSocketTransport(ICookieAuthenticatorFactory authenticatorFactory)
        {
            AuthenticatorFactory = authenticatorFactory;
        }

        #region Protected Members

        protected ICookieAuthenticatorFactory AuthenticatorFactory { get; }

        #endregion
    }
}