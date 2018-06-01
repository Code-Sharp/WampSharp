using System;

namespace WampSharp.RawSocket
{
#if !NETCORE
    [Serializable]
#endif
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