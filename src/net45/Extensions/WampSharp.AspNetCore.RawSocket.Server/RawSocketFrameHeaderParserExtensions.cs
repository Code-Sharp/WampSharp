using System;
using System.Buffers;
using WampSharp.AspNetCore.RawSocket;

namespace WampSharp.RawSocket
{
    internal static class RawSocketFrameHeaderParserExtensions
    {
        public static bool TryParse(this RawSocketFrameHeaderParser parser, in ReadOnlySequence<byte> headerBytes, out FrameType frameType, out int messageLength)
        {
            ArraySegment<byte> arraySegment = headerBytes.ToArraySegment();

            return parser.TryParse(arraySegment, out frameType, out messageLength);
        }

        public static void WriteHeader(this RawSocketFrameHeaderParser parser, FrameType frameType, int messageLength, Span<byte> span)
        {
            span[0] = (byte)frameType;

            int length = messageLength;

            for (int i = 3; i > 0; i--)
            {
                span[i] = (byte)length;
                length = length >> 8;
            }
        }
    }
}