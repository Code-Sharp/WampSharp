using System;
using System.Net.WebSockets;

namespace WampSharp.V2.Fluent
{
    public interface IWebSocketTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        IWebSocketTransportSyntax SetClientWebSocketOptions
            (Action<ClientWebSocketOptions> configureClientWebSocketOptions);

        IWebSocketTransportSyntax SetMaxFrameSize(int maxFrameSize);
    }
}