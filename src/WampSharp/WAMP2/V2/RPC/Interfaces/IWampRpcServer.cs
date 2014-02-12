using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.RPC
{
    public interface IWampRpcServer<TMessage> : IWampDealer<TMessage>, IWampRpcInvocationCallback<TMessage>
    {
    }
}