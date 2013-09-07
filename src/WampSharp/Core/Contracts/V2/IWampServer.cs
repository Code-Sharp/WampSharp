#pragma warning disable 1591

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampServer<TMessage> : IWampAuxiliaryServer<TMessage>,
                                             IWampRpcCallee<TMessage>,
                                             IWampBroker<TMessage>
    {
    }
}

#pragma warning restore 1591