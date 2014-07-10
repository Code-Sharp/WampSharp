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
}