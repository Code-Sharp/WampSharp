using System;
using System.IO;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    internal class Handshaker
    {
        public async Task<Handshake> GetHandshakeMessage(Stream stream)
        {
            byte[] bytes = new byte[4];

            await stream.ReadExactAsync(bytes, 0, bytes.Length)
                        .ConfigureAwait(false);

            ArraySegment<byte> arraySegment = new ArraySegment<byte>(bytes);

            return new Handshake(arraySegment);
        }

        public async Task SendHandshake(Stream stream, Handshake handshake)
        {
            byte[] handshakeResponse = handshake.ToArray();

            await stream.WriteAsync(handshakeResponse,
                                    0,
                                    handshakeResponse.Length)
                        .ConfigureAwait(false);
        }
    }
}