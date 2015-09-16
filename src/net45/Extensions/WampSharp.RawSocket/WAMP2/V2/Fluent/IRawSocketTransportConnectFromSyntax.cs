using System;

namespace WampSharp.V2.Fluent
{
    public interface IRawSocketTransportConnectFromSyntax : ChannelFactorySyntax.ITransportSyntax
    {
        ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan interval);
    }
}