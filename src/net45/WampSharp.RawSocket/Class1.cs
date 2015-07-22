using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace WampSharp.RawSocket
{
    public enum MessageType
    {
        WampMessage = 0,
        Ping = 1,
        Pong = 2
    }

    public class ClientToRouterRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLength">0 for 2^9, 1 for 2^10, ..., 15 for 2^24</param>
        /// <param name="serializerType"></param>
        public ClientToRouterRequest(byte maxLength, SerializerType serializerType)
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

        public ClientToRouterRequest(byte[] message)
        {
            if (message == null || message.Length != 4)
            {
                throw new ArgumentException("Expected a 4 length byte array.", "message");
            }

            MagicOctect = message[0];
            SecondOctect = message[1];
            ReservedOctects = BitConverter.ToInt16(message, 2);
        }

        // Should be 0x7F
        public byte MagicOctect { get; set; }

        public byte SecondOctect { get; set; }

        public short ReservedOctects { get; set; }

        // 0 - (1 << 9) octects
        // 1 - (1 << 10) octects
        // ...
        // 15 - (1 << 24) octects

        public int MaxMessageSizeInBytes
        {
            get
            {
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

        public byte[] ToArray()
        {
            byte[] firstOctects = {MagicOctect, SecondOctect};
            byte[] reservedBytes = BitConverter.GetBytes(ReservedOctects);

            return firstOctects.Concat(reservedBytes).ToArray();
        }
    }

    public enum SerializerType
    {
        Illegal = 0,
        Json = 1,
        MsgPack = 2
    }

    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            
            listener.Start();

            TcpClient tcpClient = listener.AcceptTcpClient();

            byte[] buffer = new byte[4];
            int readBytes = tcpClient.GetStream().Read(buffer, 0, 4);

            ClientToRouterRequest request = new ClientToRouterRequest(buffer);

            var request2 = new ClientToRouterRequest(15, SerializerType.MsgPack);
        } 
    }
}
