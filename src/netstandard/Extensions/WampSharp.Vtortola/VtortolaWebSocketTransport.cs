using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using vtortola.WebSockets.Deflate;
using vtortola.WebSockets.Rfc6455;
using WampSharp.Logging;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.Vtortola
{
    /// <summary>
    /// Represents a WebSocket transport implemented with Vtortola.
    /// </summary>
    public class VtortolaWebSocketTransport : WebSocketTransport<WebSocketData>
    {
        private readonly IPEndPoint mEndpoint;
        private WebSocketListener mListener;
        private readonly bool mPerMessageDeflate;
        private readonly X509Certificate2 mCertificate;
        private readonly WebSocketListenerOptions mOptions;
        private CancellationTokenSource mCancellationToken;

        /// <summary>
        /// Creates a new instance of <see cref="VtortolaWebSocketTransport"/>
        /// given the endpoint to run at.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="perMessageDeflate">A value indicating whether to support permessage-deflate
        /// compression extension or not.</param>
        /// <param name="options">The <see cref="WebSocketListenerOptions"/> to use for the created <see cref="WebSocketListener"/>.</param>
        public VtortolaWebSocketTransport(IPEndPoint endpoint, bool perMessageDeflate, WebSocketListenerOptions options)
            : this(endpoint, perMessageDeflate, null, options)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="VtortolaWebSocketTransport"/>
        /// given the endpoint to run at.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="perMessageDeflate">A value indicating whether to support permessage-deflate
        /// compression extension or not.</param>
        /// <param name="certificate">The <see cref="X509Certificate2"/> to use for this transport.</param>
        /// <param name="options">The <see cref="WebSocketListenerOptions"/> to use for the created <see cref="WebSocketListener"/>.</param>
        public VtortolaWebSocketTransport(IPEndPoint endpoint, bool perMessageDeflate, X509Certificate2 certificate = null, WebSocketListenerOptions options = null)
            : this(endpoint, perMessageDeflate, null, certificate, options)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="VtortolaWebSocketTransport"/>
        /// given the endpoint to run at.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="perMessageDeflate">A value indicating whether to support permessage-deflate
        ///     compression extension or not.</param>
        /// <param name="authenticatorFactory"></param>
        /// <param name="certificate"></param>
        /// <param name="options"></param>
        protected VtortolaWebSocketTransport
            (IPEndPoint endpoint, bool perMessageDeflate, ICookieAuthenticatorFactory authenticatorFactory = null, X509Certificate2 certificate = null, WebSocketListenerOptions options = null)
            : base(authenticatorFactory)
        {
            mEndpoint = endpoint;
            mPerMessageDeflate = perMessageDeflate;
            mCertificate = certificate;
            mOptions = options;
        }

        public override void Dispose()
        {
            mCancellationToken.Cancel();
            mListener.Stop();
            mListener.Dispose();
        }

        public override void Open()
        {
            string[] protocols = SubProtocols;

            WebSocketListenerOptions options = mOptions ?? new WebSocketListenerOptions();

            options.SubProtocols = protocols;
            
            WebSocketListener listener = new WebSocketListener(mEndpoint, options);

#if NETCORE
            WebSocketFactoryRfc6455 factory = new WebSocketFactoryRfc6455();
#else
            WebSocketFactoryRfc6455 factory = new WebSocketFactoryRfc6455(listener);

#endif
            if (mPerMessageDeflate)
            {
#if NETCORE
                listener.MessageExtensions.RegisterExtension(new WebSocketDeflateExtension());
#else
                factory.MessageExtensions.RegisterExtension(new WebSocketDeflateExtension());
#endif
            }

            listener.Standards.RegisterStandard(factory);
            
            if (mCertificate != null)
            {
                listener.ConnectionExtensions.RegisterExtension(new WebSocketSecureConnectionExtension(mCertificate));
            }

            mCancellationToken = new CancellationTokenSource();

#if NETCORE
            listener.StartAsync(mCancellationToken.Token);
#else
            listener.Start();
#endif

            mListener = listener;

            Task.Run(() => ListenAsync(mCancellationToken.Token));
        }

        private async Task ListenAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    WebSocket websocket = await mListener.AcceptWebSocketAsync(cancellationToken)
                        .ConfigureAwait(false);

                    if (websocket != null)
                    {
                        OnNewConnection(new WebSocketData(websocket, cancellationToken));
                    }
                }
                catch (Exception ex)
                {
                    mLogger.Error("An error occurred while trying to accept a client", ex);
                }
            }
        }

        protected override string GetSubProtocol(WebSocketData connection)
        {
            return connection.WebSocket.HttpResponse.WebSocketProtocol;
        }

        protected override void OpenConnection<TMessage>(WebSocketData original, IWampConnection<TMessage> connection)
        {
            VtortolaWampConnection<TMessage> casted = connection as VtortolaWampConnection<TMessage>;
            Task.Run(casted.HandleWebSocketAsync);
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (WebSocketData connection, IWampBinaryBinding<TMessage> binding)
        {
            return new VtortolaWampBinaryConnection<TMessage>(connection.WebSocket,
                connection.CancellationToken,
                binding,
                AuthenticatorFactory);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocketData connection, IWampTextBinding<TMessage> binding)
        {
            return new VtortolaWampTextConnection<TMessage>(connection.WebSocket,
                connection.CancellationToken,
                binding,
                AuthenticatorFactory);
        }
    }
}