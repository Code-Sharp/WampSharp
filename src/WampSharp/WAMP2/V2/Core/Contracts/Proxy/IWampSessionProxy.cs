using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionProxy<TMessage>
    {
        [WampHandler(WampMessageType.v2Hello)]
        void Hello(string realm, TMessage details);

        [WampHandler(WampMessageType.v2Abort)]
        void Abort(TMessage details, string reason);

        [WampHandler(WampMessageType.v2Authenticate)]
        void Authenticate(string signature, TMessage extra);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye(TMessage details, string reason);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int incomingSeq, int outgoingSeq);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int incomingSeq, int outgoingSeq, string discard);         
    }
}