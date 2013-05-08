using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampClient : IWampClient<object>
    {
    }

    public interface IWampClient<TMessage>
    {
        [WampHandler(WampMessageType.v1Welcome)]
        void Welcome(string sessionId, int protocolVersion, string serverIdent);

        [WampHandler(WampMessageType.v1CallResult)]
        void CallResult(string callId, TMessage result);

        [WampHandler(WampMessageType.v1CallError)]
        void CallError(string callId, string errorUri, string errorDesc);

        [WampHandler(WampMessageType.v1CallError)]
        void CallError(string callId, string errorUri, string errorDesc, TMessage errorDetails);
        
        [WampHandler(WampMessageType.v1Event)]
        void Event(string topicUri, TMessage @event);

        string SessionId { get; }
    }
}