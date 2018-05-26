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
    }
}