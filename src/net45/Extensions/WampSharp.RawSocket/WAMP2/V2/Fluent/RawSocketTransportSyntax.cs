using System;
using System.Net;
using System.Net.Sockets;

namespace WampSharp.V2.Fluent
{
    internal class RawSocketTransportSyntax : IRawSocketTransportSyntax,
        IRawSocketTransportConnectFromSyntax
    {
        public RawSocketTransportSyntax(ChannelState state)
        {
            State = state;
        }

        public ChannelState State { get; set; }

#if !NETCORE
        public IRawSocketTransportConnectFromSyntax ConnectFrom(IPEndPoint localEndPoint)
        {
            return InnerConnectFrom(() => new TcpClient(localEndPoint));
        }
#endif

        private RawSocketTransportSyntax InnerConnectFrom(Func<TcpClient> clientBuilder)
        {
            ((RawSocketActivator) State.ConnectionActivator).ClientBuilder = clientBuilder;
            return this;
        }

        public ChannelFactorySyntax.ITransportSyntax AutoPing(TimeSpan autoPingInterval)
        {
            return InnerSetInterval(autoPingInterval);
        }

        private ChannelState InnerSetInterval(TimeSpan autoPingInterval)
        {
            ((RawSocketActivator)State.ConnectionActivator).AutoPingInterval = autoPingInterval;
            return State;
        }
    }
}