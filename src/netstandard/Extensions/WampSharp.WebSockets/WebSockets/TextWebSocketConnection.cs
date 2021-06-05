using System;
using System.Net.WebSockets;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class TextWebSocketConnection<TMessage> : TextWebSocketWrapperConnection<TMessage>
    {
        public TextWebSocketConnection(WebSocket webSocket, IWampTextBinding<TMessage> binding,
                                       ICookieProvider cookieProvider,
                                       ICookieAuthenticatorFactory cookieAuthenticatorFactory, int? maxFrameSize) : base(new WebSocketWrapper(webSocket), binding, cookieProvider, cookieAuthenticatorFactory, maxFrameSize)
        {
        }

        protected TextWebSocketConnection(ClientWebSocket clientWebSocket, Uri addressUri,
                                          IWampTextBinding<TMessage> binding, int? maxFrameSize) :
            base(new ClientWebSocketWrapper(clientWebSocket), addressUri, binding, maxFrameSize)
        {
        }
    }
}