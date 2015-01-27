using System;

namespace WampSharp.V2.Client
{
    [Serializable]
    public class WampAuthenticationException : Exception
    {
        public WampAuthenticationException() { }

        public WampAuthenticationException(string message) : base(message) { }

        public WampAuthenticationException(string message, Exception inner) : base(message, inner) { }

        protected WampAuthenticationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}