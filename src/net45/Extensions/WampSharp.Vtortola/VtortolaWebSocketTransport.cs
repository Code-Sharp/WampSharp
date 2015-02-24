using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using vtortola.WebSockets.Deflate;
using vtortola.WebSockets.Rfc6455;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.Vtortola
{
    /// <summary>
    /// Represents a WebSocket transport implemented with Vtortola.
    /// </summary>
    /// TODO: This was copied and modified from Fleck implementation.
    /// TODO: Refactor these classes in order to avoid code duplication.
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
                WebSocket websocket = await mListener.AcceptWebSocketAsync(CancellationToken.None)
                    .ConfigureAwait(false);

                if (websocket != null)
                {
                    OnNewConnection(websocket);
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
            return new VtortolaWampBinaryConnection<TMessage>(connection, binding);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocket connection, IWampTextBinding<TMessage> binding)
        {
            return new VtortolaWampTextConnection<TMessage>(connection, binding);
        }
    }
}