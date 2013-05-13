namespace WampSharp.Core.Contracts.V1
{
    /// <summary>
    /// An object version of <see cref="IWampClient{TMessage}"/>
    /// </summary>
    public interface IWampClient : IWampClient<object>,
                                   IWampAuxiliaryClient,
                                   IWampRpcClient,
                                   IWampPubSubClient
    {
    }

    /// <summary>
    /// Contains all methods of WAMP client.
    /// </summary>
    public interface IWampClient<TMessage> : IWampAuxiliaryClient,
                                             IWampRpcClient<TMessage>,
                                             IWampPubSubClient<TMessage>
    {
    }
}