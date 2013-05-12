using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V1
{
    public interface IWampRpcClient : IWampRpcClient<object>
    {
    }

    public interface IWampRpcClient<TMessage>
    {
        [WampHandler(WampMessageType.v1CallResult)]
        void CallResult(string callId, TMessage result);

        [WampHandler(WampMessageType.v1CallError)]
        void CallError(string callId, string errorUri, string errorDesc);

        [WampHandler(WampMessageType.v1CallError)]
        void CallError(string callId, string errorUri, string errorDesc, TMessage errorDetails);
    }
}