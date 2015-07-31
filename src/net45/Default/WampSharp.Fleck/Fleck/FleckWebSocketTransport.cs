using System;
using System.Security.Cryptography.X509Certificates;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;
using LogLevel = Fleck.LogLevel;

namespace WampSharp.Fleck
{
    /// <summary>
    /// Represents a WebSocket transport implemented with Fleck.
    /// </summary>
    public class FleckWebSocketTransport : WebSocketTransport<IWebSocketConnection>
    {
        private readonly WebSocketServer mServer;

        /// <summary>
        /// Creates a new instance of <see cref="FleckWebSocketTransport"/>
        /// given the server address to run at.
        /// </summary>
        /// <param name="location">The given server address.</param>
        /// <param name="certificate"></param>
        public FleckWebSocketTransport(string location, X509Certificate2 certificate = null)
            : this(location, null, certificate)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FleckWebSocketTransport"/>
        /// given the server address to run at.
        /// </summary>
        /// <param name="location">The given server address.</param>
        /// <param name="cookieAuthenticatorFactory"></param>
        /// <param name="certificate"></param>
        protected FleckWebSocketTransport(string location,
                                          ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
                                          X509Certificate2 certificate = null)
            : base(cookieAuthenticatorFactory)
        {
            mServer = new WebSocketServer(location);
            mServer.Certificate = certificate;
            
            RouteLogs();
        }

        private void RouteLogs()
        {
            Action<LogLevel, string, Exception> logAction = FleckLog.LogAction;

            if (logAction != null &&
                logAction.Method.DeclaringType == typeof (FleckLog))
            {
                FleckLog.LogAction = ConvertLog;
            }
        }

        private void ConvertLog(LogLevel logLevel, string message, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                {
                    mLogger.DebugException(message, exception);
                    break;
                }
                case LogLevel.Info:
                {
                    mLogger.InfoException(message, exception);
                    break;
                }
                case LogLevel.Warn:
                {
                    mLogger.WarnException(message, exception);
                    break;
                }
                case LogLevel.Error:
                {
                    mLogger.ErrorException(message, exception);
                    break;
                }
            }
        }

        protected override void OpenConnection<TMessage>(IWampConnection<TMessage> connection)
        {
        }

        public override void Dispose()
        {
            mServer.Dispose();
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>(IWebSocketConnection connection, IWampBinaryBinding<TMessage> binding)
        {
            return new FleckWampBinaryConnection<TMessage>(connection, binding, AuthenticatorFactory);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>(IWebSocketConnection connection, IWampTextBinding<TMessage> binding)
        {
            return new FleckWampTextConnection<TMessage>(connection, binding, AuthenticatorFactory);
        }

        public override void Open()
        {
            string[] protocols = this.SubProtocols;

            mServer.SupportedSubProtocols = protocols;

            mServer.Start(OnNewConnection);
        }

        protected override string GetSubProtocol(IWebSocketConnection connection)
        {
            string protocol = connection.ConnectionInfo.NegotiatedSubProtocol;
            return protocol;
        }
    }
}