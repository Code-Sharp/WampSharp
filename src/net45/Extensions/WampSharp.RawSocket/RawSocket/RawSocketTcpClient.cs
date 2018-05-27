using System.IO;
using System.Net.Sockets;

namespace WampSharp.RawSocket
{
    public class RawSocketTcpClient
    {
        private readonly Handshake mHandshakeResponse;

        public RawSocketTcpClient(TcpClient tcpClient, Stream stream, Handshake handshakeRequest, Handshake handshakeResponse)
        {
            Client = tcpClient;
            Stream = stream;
            HandshakeRequest = handshakeRequest;
            mHandshakeResponse = handshakeResponse;
        }

        public TcpClient Client { get; }

        public Handshake HandshakeRequest { get; }

        public Handshake HandshakeResponse => mHandshakeResponse;

        public Stream Stream { get; }
    }
}