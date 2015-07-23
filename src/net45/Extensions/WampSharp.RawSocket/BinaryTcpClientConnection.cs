using System.Net.Sockets;
using WampSharp.V2.Binding;

namespace WampSharp.RawSocket
{
    public class BinaryTcpClientConnection<TMessage> : TcpClientConnection<TMessage>
    {
        internal BinaryTcpClientConnection(TcpClient client,
                                           Handshake handshake,
                                           IWampBinaryBinding<TMessage> binding)
            : base(client,
                   handshake,
                   new RawSocketBinding<TMessage>(binding))
        {
        }
    }
}