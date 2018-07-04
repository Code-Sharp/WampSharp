namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionClientTerminator
    {
        void Disconnect(GoodbyeDetails details, string reason);
    }
}