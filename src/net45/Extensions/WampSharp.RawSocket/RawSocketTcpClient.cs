using System.Net.Sockets;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    public class RawSocketTcpClient
    {
        private readonly TcpClient mTcpClient;
        public Handshake HandshakeRequest { get; private set; }

        public TcpClient Client
        {
            get
            {
                return mTcpClient;
            }
        }

        public RawSocketTcpClient(TcpClient tcpClient)
        {
            mTcpClient = tcpClient;
        }

        public async Task GetHandshakeRequest()
        {
            byte[] bytes = new byte[4];

            await Client.GetStream()
                        .ReadExactAsync(bytes, 0, bytes.Length);

            HandshakeRequest = new Handshake(bytes);
        }
    }
}