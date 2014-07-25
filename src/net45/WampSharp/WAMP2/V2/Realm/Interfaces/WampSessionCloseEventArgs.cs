using WampSharp.Core.Serialization;

namespace WampSharp.V2.Realm
{
    public abstract class WampSessionCloseEventArgs : WampSessionEventArgs
    {
        private readonly string mReason;
        private readonly SessionCloseType mCloseType;

        protected WampSessionCloseEventArgs
            (SessionCloseType closeType,
             long sessionId,
             string reason)
            : base(sessionId)
        {
            mReason = reason;
            mCloseType = closeType;
        }

        public string Reason
        {
            get
            {
                return mReason;
            }
        }

        public SessionCloseType CloseType
        {
            get
            {
                return mCloseType;
            }
        }
    }

    public class WampSessionCloseEventArgs<TMessage> : WampSessionCloseEventArgs
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly TMessage mDetails;

        public WampSessionCloseEventArgs(IWampFormatter<TMessage> formatter, SessionCloseType closeType, long sessionId, TMessage details, string reason) : 
            base(closeType, sessionId, reason)
        {
            mFormatter = formatter;
            mDetails = details;
        }

        public override T DeserializeDetails<T>()
        {
            return mFormatter.Deserialize<T>(mDetails);
        }
    }
}