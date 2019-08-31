using System;
using SuperSocket.ClientEngine;

namespace WampSharp.V2.Fluent
{
    public interface IWebSocket4NetTransportSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        ChannelFactorySyntax.ITransportSyntax SetSecurityOptions(Action<SecurityOption> configureSecurityOptions);
    }
}