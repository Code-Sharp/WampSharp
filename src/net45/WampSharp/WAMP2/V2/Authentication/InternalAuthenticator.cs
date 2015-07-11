using System;
using System.Collections.Generic;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Transports
{
    internal class InternalAuthenticator : IWampSessionAuthenticator
    {
        private readonly IWampAuthorizer mAuthorizer = new InternalConnectionAuthorizer();

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string AuthenticationId
        {
            get
            {
                return null;
            }
        }

        public string AuthenticationMethod
        {
            get
            {
                return null;
            }
        }

        public ChallengeDetails Details
        {
            get
            {
                return null;
            }
        }

        public void Authenticate(string signature, AuthenticateExtraData extra)
        {
        }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return mAuthorizer;
            }
        }

        public WelcomeDetails WelcomeDetails
        {
            get
            {
                return null;
            }
        }

        private static readonly Lazy<IWampSessionAuthenticator> mInstance =
            new Lazy<IWampSessionAuthenticator>(() => new InternalAuthenticator(), true);

        public static IWampSessionAuthenticator Instance
        {
            get
            {
                return mInstance.Value;
            }
        }

        private class InternalConnectionAuthorizer : IWampAuthorizer
        {
            public bool CanRegister(RegisterOptions options, string procedure)
            {
                return true;
            }

            public bool CanCall(CallOptions options, string procedure)
            {
                return true;
            }

            public bool CanPublish(PublishOptions options, string topicUri)
            {
                return true;
            }

            public bool CanSubscribe(SubscribeOptions options, string topicUri)
            {
                return true;
            }
        }
    }
}