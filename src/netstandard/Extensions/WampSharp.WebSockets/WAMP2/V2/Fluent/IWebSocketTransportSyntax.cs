using System;
using System.Net.WebSockets;

namespace WampSharp.V2.Fluent
{
    public interface IWebSocketTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        ChannelFactorySyntax.ITransportSyntax SetClientWebSocketOptions
            (Action<ClientWebSocketOptions> configureClientWebSocketOptions);
    }
}