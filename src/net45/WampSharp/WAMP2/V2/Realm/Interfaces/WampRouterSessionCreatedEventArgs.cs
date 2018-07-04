using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    internal class WampRouterSessionCreatedEventArgs : WampSessionCreatedEventArgs
    {
        public IWampSessionClientTerminator Terminator { get; }

        public WampRouterSessionCreatedEventArgs(long sessionId, HelloDetails helloDetails, WelcomeDetails welcomeDetails, IWampSessionClientTerminator terminator) : base(sessionId, helloDetails, welcomeDetails)
        {
            Terminator = terminator;
        }
    }
}