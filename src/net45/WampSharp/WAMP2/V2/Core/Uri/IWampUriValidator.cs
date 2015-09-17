namespace WampSharp.V2.Core
{
    public interface IWampUriValidator
    {
        bool IsValid(string uri);

        bool IsValid(string uri, string match);
    }
}