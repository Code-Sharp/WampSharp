using System;

namespace WampSharp.RawSocket
{
#if !NETCORE
    [Serializable]
#endif
    public class RawSocketProtocolException : Exception
    {
        public RawSocketProtocolException(HandshakeErrorCode errorCode) :
            base(string.Format("Server refused connection. Reason: {0}", errorCode))
        {
            ErrorCode = errorCode;
        }

        public HandshakeErrorCode ErrorCode { get; }
    }
}