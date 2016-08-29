using System;
using SuperSocket.ClientEngine;

namespace WampSharp.V2.Fluent
{
    internal class WebSocket4NetTransportSyntax : IWebSocket4NetTransportSyntax
    {
        private readonly ChannelState mState;

        public WebSocket4NetTransportSyntax(ChannelState state)
        {
            mState = state;
        }

        public ChannelState State
        {
            get
            {
                return mState;
            }
        }

        public ChannelFactorySyntax.ITransportSyntax SetSecurityOptions(Action<SecurityOption> configureSecurityOptions)
        {
            ((WebSocket4NetActivator) State.ConnectionActivator).SecurityOptionsConfigureAction = configureSecurityOptions;
            return State;
        }
    }
}