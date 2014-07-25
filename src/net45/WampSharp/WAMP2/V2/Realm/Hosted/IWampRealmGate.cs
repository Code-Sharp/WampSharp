using WampSharp.Core.Serialization;

namespace WampSharp.V2.Realm
{
    internal interface IWampRealmGate
    {
        void Hello<TMessage>(IWampFormatter<TMessage> formatter, long sessionId, TMessage details);
        void Goodbye<TMessage>(IWampFormatter<TMessage> formatter, long session, TMessage details, string reason);
        void Abort<TMessage>(IWampFormatter<TMessage> formatter, long session, TMessage details, string reason);
        void SessionLost(long sessionId);
    }
}