using System;

namespace WampSharp.Auxiliary.Client
{
    public class WampConnectionEstablishedEventArgs : EventArgs
    {
        private readonly string mSessionId;
        private readonly string mServerIdent;

        public WampConnectionEstablishedEventArgs(string sessionId,
                                                    string serverIdent)
        {
            mSessionId = sessionId;
            mServerIdent = serverIdent;
        }

        public string SessionId
        {
            get
            {
                return mSessionId;
            }
        }

        public string ServerIdent
        {
            get
            {
                return mServerIdent;
            }
        }
    }
}