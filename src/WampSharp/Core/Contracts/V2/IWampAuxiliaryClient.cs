#pragma warning disable 1591

using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampAuxiliaryClient : IWampAuxiliaryClient<object>
    {    
    }

    public interface IWampAuxiliaryClient<TMessage>
    {
        [WampHandler(WampMessageType.v2Hello)]
        void Hello(string sessionId, TMessage helloDetails);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int heartbeatSequenceNo);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat(int heartbeatSequenceNo, string discardMe);

        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye(TMessage goodbyeDetails);
    }

}
#pragma warning restore 1591