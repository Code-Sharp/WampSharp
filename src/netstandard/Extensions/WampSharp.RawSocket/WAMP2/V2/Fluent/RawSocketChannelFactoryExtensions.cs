using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WampSharp.V2.Fluent
{
    public static class RawSocketChannelFactoryExtensions
    {
        /// <summary>
        /// Indicates that the user wants to use RawSocket transport.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="address">The address of the listening router</param>
        /// <param name="port">The port of the listening router</param>
        public static IRawSocketTransportSyntax RawSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, string address, int port)
        {
            return InnerRawSocket(realmSyntax, client => client.ConnectAsync(address, port));
        }

        /// <summary>
        /// Indicates that the user wants to use RawSocket transport.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="addresses">The addresses of the listening router</param>
        /// <param name="port">The port of the listening router</param>
        public static IRawSocketTransportSyntax RawSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, IPAddress[] addresses, int port)
        {
            return InnerRawSocket(realmSyntax, client => client.ConnectAsync(addresses, port));
        }

        /// <summary>
        /// Indicates that the user wants to use RawSocket transport.
        /// </summary>
        /// <param name="realmSyntax">The current fluent syntax state.</param>
        /// <param name="address">The address of the listening router</param>
        /// <param name="port">The port of the listening router</param>
        public static IRawSocketTransportSyntax RawSocketTransport(this ChannelFactorySyntax.IRealmSyntax realmSyntax, IPAddress address, int port)
        {
            return InnerRawSocket(realmSyntax, client => client.ConnectAsync(address, port));
        }

        private static IRawSocketTransportSyntax InnerRawSocket(ChannelFactorySyntax.IRealmSyntax realmSyntax, Func<TcpClient, Task> connector)
        {
            ChannelState state = realmSyntax.State;

            RawSocketActivator activator = new RawSocketActivator();
            activator.Connector = connector;
            state.ConnectionActivator = activator;

            return new RawSocketTransportSyntax(state);
        }
    }
}