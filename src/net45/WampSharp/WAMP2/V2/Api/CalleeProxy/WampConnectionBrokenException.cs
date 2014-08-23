using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    [Serializable]
    public class WampConnectionBrokenException : Exception
    {
        public WampConnectionBrokenException()
        {
        }

        public WampConnectionBrokenException(string message) : base(message)
        {
        }

        public WampConnectionBrokenException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WampConnectionBrokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}