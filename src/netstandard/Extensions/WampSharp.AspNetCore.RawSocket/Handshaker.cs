using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading.Tasks;
using WampSharp.RawSocket;
using static WampSharp.RawSocket.RawSocketFrameHeaderParser;

namespace WampSharp.AspNetCore.RawSocket
{
    internal class Handshaker
    {
        public async Task<Handshake> GetHandshakeMessage(PipeReader input)
        {
            ReadResult readAsync = await input.ReadAsync().ConfigureAwait(false);

            if (readAsync.Buffer.Length >= FrameHeaderSize)
            {
                ReadOnlySequence<byte> handshakeBytes = 
                    readAsync.Buffer.Slice(0, FrameHeaderSize);

                ArraySegment<byte> arraySegment = handshakeBytes.ToArraySegment();

                if (Handshake.TryParse(arraySegment, out Handshake result))
                {
                    input.AdvanceTo(readAsync.Buffer.GetPosition(FrameHeaderSize));
                    return result;
                }
            }

            input.AdvanceTo(readAsync.Buffer.GetPosition(0));
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