using System;
using System.Net.WebSockets;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class BinaryWebSocketConnection<TMessage> : BinaryWebSocketWrapperConnection<TMessage>
    {
        public BinaryWebSocketConnection(WebSocket webSocket, IWampBinaryBinding<TMessage> binding,
                                         ICookieProvider cookieProvider,
                                         ICookieAuthenticatorFactory cookieAuthenticatorFactory, int? maxFrameSize) : 
            base(new WebSocketWrapper(webSocket), binding, cookieProvider, cookieAuthenticatorFactory, maxFrameSize)
        {
        }

        protected BinaryWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri,
                                            IWampBinaryBinding<TMessage> binding, int? maxFrameSize) : base(new ClientWebSocketWrapper(clientWebSocket), addressUri, binding, maxFrameSize)
        {
        }
    }
}