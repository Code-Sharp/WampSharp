using System;

namespace WampSharp.RawSocket
{
    public class RawSocketFrameHeaderParser
    {
        public const int FrameHeaderSize = 4;

        public bool TryParse(byte[] headerBytes, out FrameType frameType, out int messageLength)
        {
            ArraySegment<byte> bytes = new ArraySegment<byte>(headerBytes);
            return InnerParse(false, bytes, out frameType, out messageLength);
        }

        public bool TryParse(ArraySegment<byte> headerBytes, out FrameType frameType, out int messageLength)
        {
            return InnerParse(false, headerBytes, out frameType, out messageLength);
        }

        private static bool InnerParse(bool throwExceptions, ArraySegment<byte> headerBytes, out FrameType frameType, out int messageLength)
        {
            frameType = default(FrameType);
            messageLength = default(int);

            if (!ValidateHeaderArray(headerBytes, throwExceptions))
            {
                return false;
            }

            byte messageTypeInBytes = headerBytes.ElementAt(0);

            if (!ValidateMessageType(messageTypeInBytes, throwExceptions))
            {
                return false;
            }

            frameType = (FrameType) messageTypeInBytes;

            messageLength = headerBytes.ElementAt(3) + (headerBytes.ElementAt(2) << 8) + (headerBytes.ElementAt(1) << 16);

            return true;
        }

        private static bool ValidateHeaderArray(ArraySegment<byte> headerBytes, bool throwExceptions)
        {
            if (headerBytes.Array == null || headerBytes.Count != FrameHeaderSize)
            {
                if (throwExceptions)
                {
                    throw new ArgumentException("Expected a 4 length ArraySegment<byte>", nameof(headerBytes));
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ValidateMessageType(byte messageTypeInBytes, bool throwExceptions)
        {
            if (messageTypeInBytes > 2)
            {
                if (throwExceptions)
                {
                    throw new ArgumentException("First octect value is expected between 0 to 2", "headerBytes");
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void WriteHeader(FrameType frameType, int messageLength, byte[] array)
        {
            array[0] = (byte) frameType;

            int length = messageLength;

            for (int i = 3; i > 0; i--)
            {
                array[i] = (byte) length;
                length = length >> 8;
            }
        }
    }
}