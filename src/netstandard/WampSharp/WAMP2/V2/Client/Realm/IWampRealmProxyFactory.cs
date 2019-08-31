namespace WampSharp.V2.Client
{
    internal interface IWampRealmProxyFactory<TMessage>
    {
        IWampRealmProxy Build(WampClient<TMessage> client);
    }
}