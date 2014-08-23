using System;
using System.Runtime.Serialization;
using WampSharp.V2.Realm;

namespace WampSharp.V2.CalleeProxy
{
    [Serializable]
    public class WampConnectionBrokenException : Exception
    {
        private readonly WampSessionCloseEventArgs mEventArgs;

        public long SessionId
        {
            get { return mEventArgs.SessionId; }
        }

        public ISerializedValue Details
        {
            get { return mEventArgs.Details; }
        }

        public string Reason
        {
            get { return mEventArgs.Reason; }
        }

        public SessionCloseType CloseType
        {
            get { return mEventArgs.CloseType; }
        }

        public WampConnectionBrokenException(WampSessionCloseEventArgs eventArgs)
        {
            mEventArgs = eventArgs;
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