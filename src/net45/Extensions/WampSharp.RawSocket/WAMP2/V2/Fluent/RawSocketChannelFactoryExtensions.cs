using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WampSharp.V2.Fluent
{
    public static class RawSocketChannelFactoryExtensions
    {
        public static IRawSocketTransportSyntax RawSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address, int port)
        {
            return InnterRawSocket(realmSyntax, client => client.ConnectAsync(address, port));
        }

        public static IRawSocketTransportSyntax RawSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, IPAddress[] addresses, int port)
        {
            return InnterRawSocket(realmSyntax, client => client.ConnectAsync(addresses, port));
        }

        public static IRawSocketTransportSyntax RawSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, IPAddress address, int port)
        {
            return InnterRawSocket(realmSyntax, client => client.ConnectAsync(address, port));
        }

        private static IRawSocketTransportSyntax InnterRawSocket(ChannelFactorySyntax.IRealmSyntax realmSyntax, Func<TcpClient, Task> connector)
        {
            ChannelState state = realmSyntax.State;

            RawSocketActivator activator = new RawSocketActivator();
            activator.Connector = connector;
            state.ConnectionActivator = activator;

            return new RawSocketTransportSyntax(state);
        }
    }
}