using WampSharp.V2.Core.Contracts;
using WampSharp.V2.RPC;

namespace WampSharp.V2.Api
{
    public class WampServer<TMessage> : WampRpcServer<TMessage>, IWampServer<TMessage>
        where TMessage : class
    {
        public WampServer(IWampRpcOperationCatalog<TMessage> catalog) : 
            base(catalog)
        {
        }

        public void Hello(IWampSessionManagementClient client, string realm, TMessage details)
        {
        }

        public void Welcome(IWampSessionManagementClient client, long session, TMessage details)
        {
        }

        public void Goodbye(IWampSessionManagementClient client, string reason, TMessage details)
        {
        }

        public void Heartbeat(IWampSessionManagementClient client, int incomingSeq, int outgoingSeq)
        {
        }

        public void Heartbeat(IWampSessionManagementClient client, int incomingSeq, int outgoingSeq, string discard)
        {
        }
    }
}