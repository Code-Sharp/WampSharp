using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampRpcCaller : IWampRpcCaller<object>
    {
    }
    
    public interface IWampRpcCaller<TMessage>
    {
        [WampHandler(WampMessageType.v2CallProgress)]
        void CallProgress(string callId);

        [WampHandler(WampMessageType.v2CallProgress)]
        void CallProgress(string callId, TMessage callProgress);
        
        [WampHandler(WampMessageType.v2CallResult)]
        void CallResult(string callId);
        
        [WampHandler(WampMessageType.v2CallResult)]
        void CallResult(string callId, TMessage callResult);
        
        [WampHandler(WampMessageType.v2CallError)]
        void CallError(string callId, string error);

        [WampHandler(WampMessageType.v2CallError)]
        void CallError(string callId, string error, string errorMessage);

        [WampHandler(WampMessageType.v2CallError)]
        void CallError(string callId, string error, string errorMessage, TMessage errorDetails);
    }
}