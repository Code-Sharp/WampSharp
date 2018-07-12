using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    public interface IWampBindedRealm<TMessage>
    {
        IWampServer<TMessage> Server { get; }
        WelcomeDetails WelcomeDetails { get; }

        void Hello(IWampClientProxy<TMessage> session);
        void Abort(long session, AbortDetails details, string reason);
        void Goodbye(long session, GoodbyeDetails details, string reason);
        void SessionLost(long sessionId);
    }
}