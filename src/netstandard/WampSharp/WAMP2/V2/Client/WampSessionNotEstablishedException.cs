using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Client
{
    [Serializable]
    public class WampSessionNotEstablishedException : Exception
    {
        public WampSessionNotEstablishedException() : this("No connection to router is currently available.")
        {
        }

        public WampSessionNotEstablishedException(string message) : base(message)
        {
        }

        public WampSessionNotEstablishedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WampSessionNotEstablishedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}