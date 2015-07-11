using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Logging;
using vtortola.WebSockets;
using vtortola.WebSockets.Deflate;
using vtortola.WebSockets.Rfc6455;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.Vtortola
{
    /// <summary>
    /// Represents a WebSocket transport implemented with Vtortola.
    /// </summary>
    public class VtortolaWebSocketTransport : WebSocketTransport<WebSocket>
    {
        private readonly IPEndPoint mEndpoint;
        private WebSocketListener mListener;
        private readonly bool mPerMessageDeflate;

        /// <summary>
        /// Creates a new instance of <see cref="VtortolaWebSocketTransport"/>
        /// given the endpoint to run at.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="perMessageDeflate">A value indicating whether to support permessage-deflate
        /// compression extension or not.</param>
        public VtortolaWebSocketTransport(IPEndPoint endpoint, bool perMessageDeflate)
            : this(endpoint, perMessageDeflate, null)
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
        protected VtortolaWebSocketTransport
            (IPEndPoint endpoint,
             bool perMessageDeflate,
             ICookieAuthenticatorFactory authenticatorFactory = null) :
                 base(authenticatorFactory)
        {
            mEndpoint = endpoint;
            mPerMessageDeflate = perMessageDeflate;
        }

        public override void Dispose()
        {
            mListener.Stop();
            mListener.Dispose();
        }

        public override void Open()
        {
            string[] protocols = SubProtocols;

            WebSocketListener listener = new WebSocketListener(mEndpoint, new WebSocketListenerOptions()
            {
                SubProtocols = protocols
            });

            WebSocketFactoryRfc6455 factory = new WebSocketFactoryRfc6455(listener);

            if (mPerMessageDeflate)
            {
                factory.MessageExtensions.RegisterExtension(new WebSocketDeflateExtension());                
            }

            listener.Standards.RegisterStandard(factory);

            listener.Start();

            mListener = listener;

            Task.Run(new Func<Task>(ListenAsync));
        }

        private async Task ListenAsync()
        {
            while (mListener.IsStarted)
            {
                try
                {
                    WebSocket websocket = await mListener.AcceptWebSocketAsync(CancellationToken.None)
                        .ConfigureAwait(false);

                    if (websocket != null)
                    {
                        OnNewConnection(websocket);
                    }
                }
                catch (Exception ex)
                {
                    mLogger.Error("An error occured while trying to accept a client", ex);
                }
            }
        }

        protected override string GetSubProtocol(WebSocket connection)
        {
            return connection.HttpResponse.WebSocketProtocol;
        }

        protected override void OpenConnection<TMessage>(IWampConnection<TMessage> connection)
        {
            VtortolaWampConnection<TMessage> casted = connection as VtortolaWampConnection<TMessage>;
            Task.Run(new Func<Task>(casted.HandleWebSocketAsync));
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (WebSocket connection, IWampBinaryBinding<TMessage> binding)
        {
            return new VtortolaWampBinaryConnection<TMessage>(connection, binding, AuthenticatorFactory);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocket connection, IWampTextBinding<TMessage> binding)
        {
            return new VtortolaWampTextConnection<TMessage>(connection, binding, AuthenticatorFactory);
        }
    }
}