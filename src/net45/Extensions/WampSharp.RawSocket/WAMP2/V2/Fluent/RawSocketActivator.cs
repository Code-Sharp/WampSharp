using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.RawSocket;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Fluent
{
    internal class RawSocketActivator : IWampConnectionActivator
    {
        public RawSocketActivator()
        {
            ClientBuilder = () => new TcpClient();
        }

        public Func<TcpClient, Task> Connector { get; set; }

        public Func<TcpClient> ClientBuilder { get; set; }

        public TimeSpan? AutoPingInterval { get; set; }

        public ClientSslConfiguration SslConfiguration { get; set; }

        public IControlledWampConnection<TMessage> Activate<TMessage>(IWampBinding<TMessage> binding)
        {
            return new RawSocketClientConnection<TMessage>(ClientBuilder, Connector, (dynamic) binding, AutoPingInterval, SslConfiguration);
        }
    }
}