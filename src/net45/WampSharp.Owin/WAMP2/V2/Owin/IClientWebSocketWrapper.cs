using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.Owin
{
    public interface IClientWebSocketWrapper : IWebSocketWrapper
    {
        ClientWebSocketOptions Options { get; }
        Task ConnectAsync(Uri connectUri, CancellationToken cancellationToken);
    }
}