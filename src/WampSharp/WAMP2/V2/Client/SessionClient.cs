using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public class SessionClient<TMessage> : IWampSessionClient<TMessage>
    {
        private WampClient<TMessage> mClient;

        public SessionClient(WampClient<TMessage> client)
        {
            mClient = client;
        }

        public void Challenge(string challenge, TMessage extra)
        {
            throw new System.NotImplementedException();
        }

        public void Welcome(long session, TMessage details)
        {
            mClient.Connected();
        }

        public void Abort(TMessage details, string reason)
        {
            throw new System.NotImplementedException();
        }

        public void Goodbye(TMessage details, string reason)
        {
            throw new System.NotImplementedException();
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq)
        {
            throw new System.NotImplementedException();
        }

        public void Heartbeat(int incomingSeq, int outgoingSeq, string discard)
        {
            throw new System.NotImplementedException();
        }
    }
}