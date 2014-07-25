namespace WampSharp.V2.Realm
{
    public interface IWampHostedRealmContainer
    {
        IWampHostedRealm GetRealmByName(string name);         
    }
}