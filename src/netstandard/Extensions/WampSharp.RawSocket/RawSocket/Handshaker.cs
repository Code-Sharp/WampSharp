using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.RawSocket
{
    internal class Handshaker
    {
        public Task<Handshake> GetHandshakeMessage(Stream stream)
        {
            return GetHandshakeMessage(stream, CancellationToken.None);
        }

        public async Task<Handshake> GetHandshakeMessage(Stream stream, CancellationToken cancellationToken)
        {
            byte[] bytes = new byte[4];

            await stream.ReadExactAsync(bytes, 0, bytes.Length, cancellationToken)
                        .ConfigureAwait(false);

            ArraySegment<byte> arraySegment = new ArraySegment<byte>(bytes);

            return new Handshake(arraySegment);
        }

        public Task SendHandshake(Stream stream, Handshake handshake)
        {
            return SendHandshake(stream, handshake, CancellationToken.None);
        }

        public async Task SendHandshake(Stream stream, Handshake handshake, CancellationToken cancellationToken)
        {
            byte[] handshakeResponse = handshake.ToArray();

            await stream.WriteAsync(handshakeResponse,
                                    0,
                                    handshakeResponse.Length,
                                    cancellationToken)
                        .ConfigureAwait(false);
        }
    }
}