
using System;

namespace WampSharp.RawSocket
{
    public class WampFrameHeader
    {
        private readonly int mMessageLength;
        private readonly MessageType mMessageType;

        public WampFrameHeader(byte[] headerBytes)
        {
            if (headerBytes == null || headerBytes.Length != 4)
            {
                throw new ArgumentException("Expected a 4 length byte[]", "headerBytes");
            }

            byte messageTypeInBytes = headerBytes[0];

            if (messageTypeInBytes > 2)
            {
                throw new ArgumentException("First octect value is expected between 0 to 2", "headerBytes");
            }

            mMessageType = (MessageType)messageTypeInBytes;

            mMessageLength = headerBytes[3] + (headerBytes[2] << 8) + (headerBytes[1] << 16);
        }

        public int MessageLength
        {
            get
            {
                return mMessageLength;
            }
        }

        public MessageType MessageType
        {
            get
            {
                return mMessageType;
            }
        }
    }
}