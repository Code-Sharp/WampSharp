using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    public interface IWampBindedRealm<TMessage>
    {
        IWampServer<TMessage> Server { get; }
        
        void Hello(long session, TMessage details);
        void Abort(long session, TMessage details, string reason);
        void Goodbye(long session, TMessage details, string reason);
        void SessionLost(long sessionId);
    }
}