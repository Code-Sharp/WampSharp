using System;

namespace WampSharp.RawSocket
{

    [Serializable]
    public class RawSocketProtocolException : Exception
    {
        private readonly HandshakeErrorCode mErrorCode;

        public RawSocketProtocolException(HandshakeErrorCode errorCode) :
            base(string.Format("Server refused connection. Reason: {0}", errorCode))
        {
            mErrorCode = errorCode;
        }

        public HandshakeErrorCode ErrorCode
        {
            get { return mErrorCode; }
        }
    }
}