using System.Net.Sockets;
using WampSharp.V2.Binding;

namespace WampSharp.RawSocket
{
    public class TextTcpClientConnection<TMessage> : TcpClientConnection<TMessage>
    {
        internal TextTcpClientConnection(TcpClient client,
                                         Handshake handshake,
                                         IWampTextBinding<TMessage> binding)
            : base(client,
                   handshake,
                   new RawSocketBinding<TMessage>(binding))
        {
        }
    }
}