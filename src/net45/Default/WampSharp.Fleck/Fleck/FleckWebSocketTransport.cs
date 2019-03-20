using System;
using System.Security.Authentication;
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
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        public FleckWebSocketTransport(string location, X509Certificate2 certificate = null, bool supportDualStack = true)
            : this(location: location, supportDualStack: supportDualStack, cookieAuthenticatorFactory: null, certificate: certificate, getEnabledSslProtocols: null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FleckWebSocketTransport"/>
        /// given the server address to run at.
        /// </summary>
        /// <param name="location">The given server address.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="getEnabledSslProtocols"> If non-null, used to set Fleck's EnabledSslProtocols. </param>
        public FleckWebSocketTransport(string location, X509Certificate2 certificate, Func<SslProtocols> getEnabledSslProtocols, bool supportDualStack = true)
            : this(location: location, supportDualStack: supportDualStack, cookieAuthenticatorFactory: null, certificate: certificate, getEnabledSslProtocols: getEnabledSslProtocols)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FleckWebSocketTransport"/>
        /// given the server address to run at.
        /// </summary>
        /// <param name="location">The given server address.</param>
        /// <param name="cookieAuthenticatorFactory"></param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        protected FleckWebSocketTransport(string location,
                                          ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
                                          X509Certificate2 certificate = null,
                                          bool supportDualStack = true)
            : this(location: location, cookieAuthenticatorFactory: null, certificate: certificate, supportDualStack: supportDualStack, getEnabledSslProtocols: null)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FleckWebSocketTransport"/>
        /// given the server address to run at.
        /// </summary>
        /// <param name="location">The given server address.</param>
        /// <param name="cookieAuthenticatorFactory"></param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> certificate to use for secured websockets.</param>
        /// <param name="supportDualStack">IPv4/IPv6 dual stack support</param>
        /// <param name="getEnabledSslProtocols"> If non-null, used to set Fleck's EnabledSslProtocols. </param>
        protected FleckWebSocketTransport(string location,
                                          ICookieAuthenticatorFactory cookieAuthenticatorFactory = null,
                                          X509Certificate2 certificate = null,
                                          Func<SslProtocols> getEnabledSslProtocols = null,
                                          bool supportDualStack = true)
            : base(cookieAuthenticatorFactory)
        {
            mServer = new WebSocketServer(location, supportDualStack);
            mServer.Certificate = certificate;

            if (getEnabledSslProtocols != null)
            {
                mServer.EnabledSslProtocols = getEnabledSslProtocols();
            }

            RouteLogs();
        }

        private void RouteLogs()
        {
            Action<LogLevel, string, Exception> logAction = FleckLog.LogAction;

            if (logAction != null &&
                GetMethodDeclaringType(logAction) == typeof (FleckLog))
            {
                FleckLog.LogAction = ConvertLog;
            }
        }

        private static Type GetMethodDeclaringType(Action<LogLevel, string, Exception> logAction)
        {
            Type methodDeclaringType = logAction.Method.DeclaringType;

            while (methodDeclaringType.IsNested)
            {
                methodDeclaringType = methodDeclaringType.DeclaringType;
            }

            return methodDeclaringType;
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

        protected override void OpenConnection<TMessage>(IWebSocketConnection original, IWampConnection<TMessage> connection)
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