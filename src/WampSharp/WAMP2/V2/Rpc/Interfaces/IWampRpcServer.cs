using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcServer<TMessage> : IWampDealer<TMessage>,
                                                IWampRpcInvocationCallback<TMessage>,
                                                IWampErrorCallback<TMessage>
    {
    }
}