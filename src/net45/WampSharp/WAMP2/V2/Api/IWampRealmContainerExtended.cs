namespace WampSharp.V2
{
    public interface IWampRealmContainerExtended
    {
        IWampRealmExtended GetRealmByName(string name);
    }
}