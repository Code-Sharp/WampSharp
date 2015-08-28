using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Client
{
    [Serializable]
    public class WampSessionNotEstablishedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

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