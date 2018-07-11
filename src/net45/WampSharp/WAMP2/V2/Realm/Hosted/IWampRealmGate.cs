using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    internal interface IWampRealmGate
    {
        void Hello(long sessionId, HelloDetails helloDetails, WelcomeDetails welcomeDetails, IWampSessionTerminator terminator);
        void Goodbye(long session, GoodbyeDetails details, string reason);
        void Abort(long session, AbortDetails details, string reason);
        void SessionLost(long sessionId);
    }
}