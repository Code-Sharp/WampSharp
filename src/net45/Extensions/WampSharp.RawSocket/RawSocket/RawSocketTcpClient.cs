using System.IO;
using System.Net.Sockets;

namespace WampSharp.RawSocket
{
    public class RawSocketTcpClient
    {
        private readonly TcpClient mTcpClient;
        private readonly Stream mStream;
        private readonly Handshake mHandshakeRequest;
        private readonly Handshake mHandshakeResponse;

        public RawSocketTcpClient(TcpClient tcpClient, Stream stream, Handshake handshakeRequest, Handshake handshakeResponse)
        {
            mTcpClient = tcpClient;
            mStream = stream;
            mHandshakeRequest = handshakeRequest;
            mHandshakeResponse = handshakeResponse;
        }

        public TcpClient Client
        {
            get
            {
                return mTcpClient;
            }
        }

        public Handshake HandshakeRequest
        {
            get
            {
                return mHandshakeRequest;
            }
        }

        public Handshake HandshakeResponse
        {
            get
            {
                return mHandshakeResponse;
            }
        }

        public Stream Stream
        {
            get { return mStream; }
        }
    }
}