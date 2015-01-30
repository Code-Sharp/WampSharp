using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    [Serializable]
    public class WampAuthenticationException : Exception
    {
        private const string WampErrorCannotAuthenticate = "wamp.error.cannot_authenticate";
        private readonly AbortDetails mDetails;
        private readonly string mReason;

        public WampAuthenticationException(string message = "sorry, I cannot authenticate (onchallenge handler raised an exception)", string reason = WampErrorCannotAuthenticate)
            : this(new AbortDetails() { Message = message}, reason)
        {
        }

        public WampAuthenticationException(AbortDetails details, string reason = WampErrorCannotAuthenticate)
            : base(details.Message)
        {
            mDetails = details;
            mReason = reason;
        }

        protected WampAuthenticationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WampAuthenticationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context, AbortDetails details, string reason)
            : base(info, context)
        {
        }

        public AbortDetails Details
        {
            get { return mDetails; }
        }

        public string Reason
        {
            get { return mReason; }
        }
    }
}