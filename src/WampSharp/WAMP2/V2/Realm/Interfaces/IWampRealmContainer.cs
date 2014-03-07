namespace WampSharp.V2.Realm
{
    public interface IWampRealmContainer<TMessage>
        where TMessage : class
    {
        IWampRealm<TMessage> GetRealmByName(string name);
    }
}