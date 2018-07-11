namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSessionTerminator
    {
        void Disconnect(GoodbyeDetails details, string reason);
    }
}