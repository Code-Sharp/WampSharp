using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampRpcCallee<TMessage>
    {
        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampRpcCaller caller, string callId, string endpoint);
        
        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampRpcCaller caller, string callId, string endpoint, TMessage[] arguments);
        
        [WampHandler(WampMessageType.v2Call)]
        void Call([WampProxyParameter] IWampRpcCaller caller, string callId, string endpoint, TMessage[] arguments, TMessage callOptions);
        
        [WampHandler(WampMessageType.v2CallCancel)]
        void CallCancel([WampProxyParameter] IWampRpcCaller caller, string callId, string endpoint);
        
        [WampHandler(WampMessageType.v2CallCancel)]
        void CallCancel([WampProxyParameter] IWampRpcCaller caller, string callId, string endpoint, TMessage callCancelOptions);
    }
}