using System;

namespace WampSharp.RawSocket
{
    internal class RawSocketFrameHeaderParser
    {
        public void Parse(byte[] headerBytes, out FrameType frameType, out int messageLength)
        {
            InnerParse(true, headerBytes, out frameType, out messageLength);
        }

        public bool TryParse(byte[] headerBytes, out FrameType frameType, out int messageLength)
        {
            return InnerParse(false, headerBytes, out frameType, out messageLength);
        }

        private static bool InnerParse(bool throwExceptions, byte[] headerBytes, out FrameType frameType, out int messageLength)
        {
            frameType = default(FrameType);
            messageLength = default(int);

            if (!ValidateHeaderArray(headerBytes, throwExceptions))
            {
                return false;
            }

            byte messageTypeInBytes = headerBytes[0];

            if (!ValidateMessageType(messageTypeInBytes, throwExceptions))
            {
                return false;
            }

            frameType = (FrameType) messageTypeInBytes;

            messageLength = headerBytes[3] + (headerBytes[2] << 8) + (headerBytes[1] << 16);

            return true;
        }

        private static bool ValidateHeaderArray(byte[] headerBytes, bool throwExceptions)
        {
            if (headerBytes == null || headerBytes.Length != 4)
            {
                if (throwExceptions)
                {
                    throw new ArgumentException("Expected a 4 length byte[]", "headerBytes");
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