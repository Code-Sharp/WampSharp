using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    public interface IWampRpcServer<TMessage>
    {
        [WampHandler(WampMessageType.Call)]
        void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments);         
    }
}