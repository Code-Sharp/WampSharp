using System.Buffers;

namespace WampSharp.RawSocket
{
    internal static class RawSocketFrameHeaderParserExtensions
    {
        public static bool TryParse(this RawSocketFrameHeaderParser parser, in ReadOnlySequence<byte> headerBytes, out FrameType frameType, out int messageLength)
        {
            // TODO: implement better
            return parser.TryParse(headerBytes.ToArray(), out frameType, out messageLength);
        }
    }
}