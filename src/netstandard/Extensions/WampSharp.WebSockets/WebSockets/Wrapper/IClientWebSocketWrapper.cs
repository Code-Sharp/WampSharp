using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.WebSockets
{
    public interface IClientWebSocketWrapper : IWebSocketWrapper
    {
        ClientWebSocketOptions Options { get; }
        Task ConnectAsync(Uri connectUri, CancellationToken cancellationToken);
    }
}