using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Respresents <see cref="EventArgs"/> for a WAMP session event.
    /// </summary>
    public class WampSessionCreatedEventArgs : EventArgs
    {
        public WampSessionCreatedEventArgs(long sessionId, HelloDetails helloDetails, WelcomeDetails welcomeDetails, IWampSessionTerminator terminator)
        {
            SessionId = sessionId;
            HelloDetails = helloDetails;
            WelcomeDetails = welcomeDetails;
            Terminator = terminator;
        }

        /// <summary>
        /// Gets the relevant session id.
        /// </summary>
        public long SessionId { get; }

        public HelloDetails HelloDetails { get; }

        public WelcomeDetails WelcomeDetails { get; }

        public IWampSessionTerminator Terminator { get; }
    }
}