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
        private readonly long mSessionId;
        private readonly HelloDetails mHelloDetails;
        private readonly WelcomeDetails mWelcomeDetails;

        public WampSessionCreatedEventArgs(long sessionId, HelloDetails helloDetails, WelcomeDetails welcomeDetails)
        {
            mSessionId = sessionId;
            mHelloDetails = helloDetails;
            mWelcomeDetails = welcomeDetails;
        }

        /// <summary>
        /// Gets the relevant session id.
        /// </summary>
        public long SessionId
        {
            get
            {
                return mSessionId;
            }
        }

        public HelloDetails HelloDetails
        {
            get
            {
                return mHelloDetails;
            }
        }

        public WelcomeDetails WelcomeDetails
        {
            get
            {
                return mWelcomeDetails;
            }
        }
    }
}