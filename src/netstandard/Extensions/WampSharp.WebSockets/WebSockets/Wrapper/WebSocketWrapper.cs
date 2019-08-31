using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.WebSockets
{
    internal class WebSocketWrapper : IWebSocketWrapper
    {
        private readonly WebSocket mWebSocket;

        public WebSocketWrapper(WebSocket webSocket)
        {
            mWebSocket = webSocket;
        }

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> arraySegment, CancellationToken callCancelled)
        {
            return mWebSocket.ReceiveAsync(arraySegment, callCancelled);
        }

        public Task SendAsync(ArraySegment<byte> data, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancel)
        {
            return mWebSocket.SendAsync(data, messageType, endOfMessage, cancel);
        }

        public Task CloseAsync(WebSocketCloseStatus closeStatus, string closeDescription, CancellationToken cancel)
        {
            return mWebSocket.CloseAsync(closeStatus, closeDescription, cancel);
        }

        public WebSocketState State => mWebSocket.State;
    }
}