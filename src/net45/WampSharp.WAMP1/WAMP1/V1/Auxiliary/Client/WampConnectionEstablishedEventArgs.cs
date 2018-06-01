using System;

namespace WampSharp.V1.Auxiliary.Client
{
    public class WampConnectionEstablishedEventArgs : EventArgs
    {
        private readonly string mServerIdent;

        public WampConnectionEstablishedEventArgs(string sessionId,
                                                    string serverIdent)
        {
            SessionId = sessionId;
            mServerIdent = serverIdent;
        }

        public string SessionId { get; }

        public string ServerIdent => mServerIdent;
    }
}