using System;
using System.Runtime.Serialization;

namespace WampSharp.Rpc
{
    [Serializable]
    public class WampRpcCallException : Exception
    {
        private readonly string mProcUri;
        private readonly string mCallId;
        private readonly string mErrorUri;
        private readonly object mErrorDetails;

        public WampRpcCallException(string errorUri, string errorDesc, object errorDetails)
            : this(null, null, errorUri, errorDesc, errorDetails)
        {
        }

        public WampRpcCallException(string procUri, string callId, string errorUri, string errorDesc, object errorDetails)
            : base(errorDesc)
        {
            mProcUri = procUri;
            mCallId = callId;
            mErrorUri = errorUri;
            mErrorDetails = errorDetails;
        }

        protected WampRpcCallException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public string ProcUri
        {
            get
            {
                return mProcUri;
            }
        }

        public string CallId
        {
            get
            {
                return mCallId;
            }
        }

        public string ErrorUri
        {
            get
            {
                return mErrorUri;
            }
        }

        public object ErrorDetails
        {
            get
            {
                return mErrorDetails;
            }
        }
    }
}