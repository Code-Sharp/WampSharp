using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampRpcServer<TMessage>
    {
        [WampHandler(WampMessageType.v1Call)]
        void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments);         
    }
}