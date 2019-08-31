namespace WampSharp.V2.Realm.Binded
{
    internal interface IWampBindedRealmContainer<TMessage>
    {
        IWampBindedRealm<TMessage> GetRealmByName(string name);
    }
}