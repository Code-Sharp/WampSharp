using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Session
{
    public class WampSessionServer<TMessage> : IWampSessionServer<TMessage>
    {
        public void OnNewClient(IWampClient client)
        {
            client.Welcome(client.Session, null);
        }

        public void Hello(IWampSessionClient client, string realm, TMessage details)
        {
        }

        public void Authenticate(IWampSessionClient client, string signature, TMessage extra)
        {
        }

        public void Welcome(IWampSessionClient client, long session, TMessage details)
        {
        }

        public void Goodbye(IWampSessionClient client, string reason, TMessage details)
        {
        }

        public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq)
        {
        }

        public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq, string discard)
        {
        }
    }
}