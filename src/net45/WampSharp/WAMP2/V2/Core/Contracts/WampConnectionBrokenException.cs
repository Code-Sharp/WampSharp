using System;
using System.Runtime.Serialization;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampConnectionBrokenException : Exception
    {
        private readonly WampSessionCloseEventArgs mEventArgs;

        public long SessionId
        {
            get { return mEventArgs.SessionId; }
        }

        public GoodbyeAbortDetails Details
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
            : base(GetExceptionMessage(eventArgs))
        {
            mEventArgs = eventArgs;
        }

        private static string GetExceptionMessage(WampSessionCloseEventArgs eventArgs)
        {
            string result = 
                string.Format("Connection got broken. CloseType:{0}", eventArgs.CloseType);
            
            string reason = eventArgs.Reason;
            
            if (reason != null)
            {
                result = string.Format("{0}, Reason: {1}", result, reason);
            }

            return result;
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