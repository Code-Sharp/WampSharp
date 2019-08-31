using System.Net.WebSockets;

namespace WampSharp.WebSockets
{
    /// <summary>
    /// A delegate that creates a new instance of a <see cref="WebSocket"/>.
    /// </summary>
    public delegate ClientWebSocket WebSocketFactory();
}