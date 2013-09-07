#pragma warning disable 1591

using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampAuxiliaryServer<TMessage>
    {
        [WampHandler(WampMessageType.v2Hello)]
        void Hello([WampProxyParameter] IWampAuxiliaryClient client, string sessionId, TMessage helloDetails);
        
        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat([WampProxyParameter] IWampAuxiliaryClient client, int heartbeatSequenceNo);

        [WampHandler(WampMessageType.v2Heartbeat)]
        void Heartbeat([WampProxyParameter] IWampAuxiliaryClient client, int heartbeatSequenceNo, string discardMe);
        
        [WampHandler(WampMessageType.v2Goodbye)]
        void Goodbye([WampProxyParameter] IWampAuxiliaryClient client, TMessage goodbyeDetails);
    }
}

#pragma warning restore 1591