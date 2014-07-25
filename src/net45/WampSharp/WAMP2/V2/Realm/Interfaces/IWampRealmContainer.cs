namespace WampSharp.V2.Realm
{
    public interface IWampRealmContainer
    {
        IWampRealm GetRealmByName(string name);
    }
}