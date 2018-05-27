using System;
using SuperSocket.ClientEngine;

namespace WampSharp.V2.Fluent
{
    internal class WebSocket4NetTransportSyntax : IWebSocket4NetTransportSyntax
    {
        public WebSocket4NetTransportSyntax(ChannelState state)
        {
            State = state;
        }

        public ChannelState State { get; }

        public ChannelFactorySyntax.ITransportSyntax SetSecurityOptions(Action<SecurityOption> configureSecurityOptions)
        {
            ((WebSocket4NetActivator) State.ConnectionActivator).SecurityOptionsConfigureAction = configureSecurityOptions;
            return State;
        }
    }
}