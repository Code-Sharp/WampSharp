using System;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm
{
    /// <summary>
    /// Respresents <see cref="EventArgs"/> for a WAMP session event.
    /// </summary>
    public class WampSessionCreatedEventArgs : EventArgs
    {
        private readonly WelcomeDetails mWelcomeDetails;

        public WampSessionCreatedEventArgs(long sessionId, HelloDetails helloDetails, WelcomeDetails welcomeDetails)
        {
            SessionId = sessionId;
            HelloDetails = helloDetails;
            mWelcomeDetails = welcomeDetails;
        }

        /// <summary>
        /// Gets the relevant session id.
        /// </summary>
        public long SessionId { get; }

        public HelloDetails HelloDetails { get; }

        public WelcomeDetails WelcomeDetails
        {
            get
            {
                return mWelcomeDetails;
            }
        }
    }
}