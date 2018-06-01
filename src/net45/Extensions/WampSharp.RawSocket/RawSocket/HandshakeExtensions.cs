using System.Collections.Generic;

namespace WampSharp.RawSocket
{
    public static class HandshakeExtensions
    {
        public static Handshake GetHandshakeResponse(this Handshake handshakeRequest, ICollection<string> subProtocols, byte maxSize)
        {
            Handshake handshakeResponse;

            if (handshakeRequest.ReservedOctets != 0)
            {
                handshakeResponse = new Handshake(HandshakeErrorCode.UseOfReservedBits);
            }
            else
            {
                SerializerType serializerType = handshakeRequest.SerializerType;

                string requestedSubprotocol = serializerType.GetSubProtocol();

                if (!subProtocols.Contains(requestedSubprotocol))
                {
                    handshakeResponse = new Handshake(HandshakeErrorCode.SerializerUnsupported);
                }
                else
                {
                    handshakeResponse = new Handshake(maxSize, serializerType);
                }
            }

            return handshakeResponse;
        }
    }
}