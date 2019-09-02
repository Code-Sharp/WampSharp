using System;

namespace WampSharp.RawSocket
{
    [Serializable]
    public class RawSocketProtocolException : Exception
    {
        public RawSocketProtocolException(HandshakeErrorCode errorCode) :
            base($"Server refused connection. Reason: {errorCode}")
        {
            ErrorCode = errorCode;
        }

        public HandshakeErrorCode ErrorCode { get; }
    }
}