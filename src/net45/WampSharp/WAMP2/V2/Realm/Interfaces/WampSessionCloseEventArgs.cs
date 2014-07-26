namespace WampSharp.V2.Realm
{
    public class WampSessionCloseEventArgs : WampSessionEventArgs
    {
        private readonly string mReason;
        private readonly SessionCloseType mCloseType;

        public WampSessionCloseEventArgs
            (SessionCloseType closeType,
             long sessionId,
             ISerializedValue details,
             string reason)
            : base(sessionId, details)
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
}