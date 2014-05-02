using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampException : Exception
    {
        private readonly string mErrorUri;
        private readonly object mDetails;

        public WampException(string errorUri, object details)
        {
            mErrorUri = errorUri;
            mDetails = details;
        }

        public WampException(string errorUri, object details, string message) : base(message)
        {
            mErrorUri = errorUri;
            mDetails = details;
        }

        public WampException(string errorUri, object details, string message, Exception inner) : base(message, inner)
        {
            mErrorUri = errorUri;
            mDetails = details;
        }

        protected WampException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public string ErrorUri
        {
            get
            {
                return mErrorUri;
            }
        }

        public object Details
        {
            get
            {
                return mDetails;
            }
        }
    }
}