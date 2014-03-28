using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionClient
    {        
        [WampHandler(WampMessageType.v2Challenge)]
        void Challenge(string challenge, object extra);

        [WampHandler(WampMessageType.v2Welcome)]
        void Welcome(long session, object details);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye(string reason, object details);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int incomingSeq, int outgoingSeq);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int incomingSeq, int outgoingSeq, string discard);    
    }
}