namespace WampSharp.V2.Core.Contracts
{
    internal interface IWampSessionClientTerminator
    {
        void Disconnect(GoodbyeDetails details, string reason);
    }
}