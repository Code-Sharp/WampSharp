using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    public interface IWampClient : IWampClient<object>
    {
    }

    public interface IWampClient<TMessage>
    {
        [WampHandler(WampMessageType.Welcome)]
        void Welcome(string sessionId, int protocolVersion, string serverIdent);

        [WampHandler(WampMessageType.CallResult)]
        void CallResult(string callId, TMessage result);

        [WampHandler(WampMessageType.CallError)]
        void CallError(string callId, string errorUri, string errorDesc);

        [WampHandler(WampMessageType.CallError)]
        void CallError(string callId, string errorUri, string errorDesc, TMessage errorDetails);
        
        [WampHandler(WampMessageType.Event)]
        void Event(string topicUri, TMessage @event);

        string SessionId { get; }
    }
}