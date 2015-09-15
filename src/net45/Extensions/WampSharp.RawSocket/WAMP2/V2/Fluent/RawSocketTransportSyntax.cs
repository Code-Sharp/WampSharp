using System;
using System.Net;
using System.Net.Sockets;

namespace WampSharp.V2.Fluent
{
    internal class RawSocketTransportSyntax : IRawSocketTransportSyntax
    {
        public RawSocketTransportSyntax(ChannelState state)
        {
            State = state;
        }

        public ChannelState State { get; set; }

        public ChannelFactorySyntax.ITransportSyntax ConnectFrom(AddressFamily family)
        {
            return InnerConnectFrom(() => new TcpClient(family));
        }

        public ChannelFactorySyntax.ITransportSyntax ConnectFrom(IPEndPoint localEndPoint)
        {
            return InnerConnectFrom(() => new TcpClient(localEndPoint));
        }

        public ChannelFactorySyntax.ITransportSyntax ConnectFrom(string hostname, int port)
        {
            return InnerConnectFrom(() => new TcpClient(hostname, port));
        }

        private ChannelState InnerConnectFrom(Func<TcpClient> clientBuilder)
        {
            ((RawSocketActivator) State.ConnectionActivator).ClientBuilder = clientBuilder;
            return State;
        }
    }
}