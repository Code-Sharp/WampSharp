using System;
using System.Runtime.Serialization;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampConnectionBrokenException : Exception
    {
        private readonly WampSessionCloseEventArgs mEventArgs;

        public long SessionId => mEventArgs.SessionId;

        public GoodbyeAbortDetails Details => mEventArgs.Details;

        public string Reason => mEventArgs.Reason;

        public SessionCloseType CloseType => mEventArgs.CloseType;

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

#if !PCL
        protected WampConnectionBrokenException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}