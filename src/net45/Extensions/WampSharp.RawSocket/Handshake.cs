using System;
using System.Linq;

namespace WampSharp.RawSocket
{
    public class Handshake
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLength">0 for 2^9, 1 for 2^10, ..., 15 for 2^24</param>
        /// <param name="serializerType"></param>
        public Handshake(byte maxLength, SerializerType serializerType)
        {
            MagicOctect = 0x7F;

            if (maxLength >= 16)
            {
                throw new ArgumentOutOfRangeException("maxLength", "Expected a value between 0 to 15");
            }

            if (serializerType != SerializerType.Json && 
                serializerType != SerializerType.MsgPack)
            {
                throw new ArgumentException("Expected a value between 1 to 2", "serializerType");
            }

            SecondOctect = (byte) ((maxLength << 4) + (byte)serializerType);
        }

        public Handshake(HandshakeErrorCode errorCode)
        {
            MagicOctect = 0x7F;

            if (errorCode == HandshakeErrorCode.Illegal)
            {
                throw new ArgumentException("HandshakeErrorCode.Illegal must not be used",
                                            "errorCode");
            }

            SecondOctect = (byte)(((byte)errorCode) << 4);
        }

        public Handshake(byte[] message)
        {
            if (message == null || message.Length != 4)
            {
                throw new ArgumentException("Expected a 4 length byte array.", "message");
            }

            MagicOctect = message[0];

            if (MagicOctect != 0x7F)
            {
                throw new ArgumentException("First octect must be 0x7F.", "message");
            }

            SecondOctect = message[1];

            ReservedOctects = BitConverter.ToInt16(message, 2);

            if (ReservedOctects != 0)
            {
                throw new ArgumentException("Reserved octects must be zeros.", "message");
            }
        }

        // Should be 0x7F
        public byte MagicOctect { get; set; }

        public byte SecondOctect { get; set; }

        public short ReservedOctects { get; set; }

        public int MaxMessageSizeInBytes
        {
            get
            {
                // 0 - (1 << 9) octects
                // 1 - (1 << 10) octects
                // ...
                // 15 - (1 << 24) octects
                int sizeHalfByte = SecondOctect >> 4;
                int size = sizeHalfByte + 9;
                return 1 << size;
            }
        }

        public SerializerType SerializerType
        {
            get
            {
                int serializerHalfByte = SecondOctect & 0x0F;
                return (SerializerType) serializerHalfByte;
            }
        }

        public HandshakeErrorCode? HandshakeError
        {
            get
            {
                if (!IsError)
                {
                    return null;
                }

                int errorHalfByte = SecondOctect >> 4;

                return (HandshakeErrorCode) errorHalfByte;
            }
        }

        public bool IsError
        {
            get
            {
                return (SerializerType == SerializerType.Illegal);
            }
        }

        public byte[] ToArray()
        {
            byte[] firstOctects = {MagicOctect, SecondOctect};
            byte[] reservedBytes = BitConverter.GetBytes(ReservedOctects);

            return firstOctects.Concat(reservedBytes).ToArray();
        }

        public Handshake GetAcceptedResponse(byte maxLength)
        {
            return new Handshake(maxLength, SerializerType);
        }

        public Handshake GetErrorResponse(HandshakeErrorCode errorCode)
        {
            return new Handshake(errorCode);
        }
    }

    public enum HandshakeErrorCode
    {
        Illegal = 0,
        SerializerUnsupported = 1,
        MaximumMessageLengthUnacceptable = 2,
        UseOfReservedBits = 3,
        MaximumConnectionCountReached = 4
    }
}