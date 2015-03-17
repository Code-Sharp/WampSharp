using WampSharp.Core.Serialization;
using WampSharp.V2.Reflection;

namespace WampSharp.V2.Realm
{
    internal interface IWampRealmGate
    {
        void Hello<TMessage>(IWampFormatter<TMessage> formatter, long sessionId, WampTransportDetails transportDetails, TMessage details);
        void Goodbye<TMessage>(IWampFormatter<TMessage> formatter, long session, TMessage details, string reason);
        void Abort<TMessage>(IWampFormatter<TMessage> formatter, long session, TMessage details, string reason);
        void SessionLost(long sessionId);
    }
}