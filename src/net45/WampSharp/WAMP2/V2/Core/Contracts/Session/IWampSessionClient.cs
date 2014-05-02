using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionClient : IWampSessionClient<object>
    {
    }

    public interface IWampSessionClient<TMessage>
    {        
        [WampHandler(WampMessageType.v2Challenge)]
        void Challenge(string challenge, TMessage extra);

        [WampHandler(WampMessageType.v2Welcome)]
        void Welcome(long session, TMessage details);

        [WampHandler(WampMessageType.v2Abort)]
        void Abort(TMessage details, string reason);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye(TMessage details, string reason);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int incomingSeq, int outgoingSeq);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int incomingSeq, int outgoingSeq, string discard);    
    }
}