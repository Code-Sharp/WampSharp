using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;
using WampSharp.RawSocket;

namespace WampSharp.AspNetCore.RawSocket
{
    internal class Handshaker
    {
        public async Task<Handshake> GetHandshakeMessage(PipeReader input)
        {
            ReadResult readAsync = await input.ReadAsync().ConfigureAwait(false);
            ReadOnlySequence<byte> handshakeBytes = readAsync.Buffer.Slice(0, 4);

            Handshake result;

            ArraySegment<byte> arraySegment = handshakeBytes.ToArraySegment();

            if (!Handshake.TryParse(arraySegment, out result))
            {
                input.AdvanceTo(readAsync.Buffer.GetPosition(0));
            }
            else
            {
                input.AdvanceTo(readAsync.Buffer.GetPosition(4));
                return result;
            }

            return null;
        }

        public async Task SendHandshake(PipeWriter output, Handshake response)
        {
            byte[] responseBytes = response.ToArray();

            await output.WriteAsync(responseBytes)
                        .ConfigureAwait(false);
        }
    }
}