using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionManagementServer<TMessage>
    {
        [WampHandler(WampMessageType.v2Hello)]
        void Hello([WampProxyParameter]IWampSessionManagementClient client, string realm, TMessage details);

        [WampHandler(WampMessageType.v2Welcome)]
        void Welcome([WampProxyParameter]IWampSessionManagementClient client, long session, TMessage details);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye([WampProxyParameter]IWampSessionManagementClient client, string reason, TMessage details);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat([WampProxyParameter]IWampSessionManagementClient client, int incomingSeq, int outgoingSeq);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat([WampProxyParameter]IWampSessionManagementClient client, int incomingSeq, int outgoingSeq, string discard);
    }
}