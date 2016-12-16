using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.Owin
{
    public interface IWebSocketWrapper
    {
        CancellationToken CancellationToken { get; }
        bool IsConnected { get; }
        Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> arraySegment, CancellationToken callCancelled);
        Task SendAsync(ArraySegment<byte> data, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancel);
        Task CloseAsync(WebSocketCloseStatus closeStatus, string closeDescription, CancellationToken cancel);
    }
}