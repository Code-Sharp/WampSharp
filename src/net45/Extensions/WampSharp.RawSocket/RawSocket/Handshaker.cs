using System.Net.Sockets;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    internal class Handshaker
    {
        public async Task<Handshake> GetHandshakeMessage(TcpClient client)
        {
            byte[] bytes = new byte[4];

            await client.GetStream()
                        .ReadExactAsync(bytes, 0, bytes.Length)
                        .ConfigureAwait(false);

            return new Handshake(bytes);
        }

        public async Task SendHandshake(TcpClient client, Handshake handshake)
        {
            byte[] handshakeResponse = handshake.ToArray();

            await client.GetStream()
                        .WriteAsync(handshakeResponse,
                                    0,
                                    handshakeResponse.Length)
                        .ConfigureAwait(false);
        }
    }
}