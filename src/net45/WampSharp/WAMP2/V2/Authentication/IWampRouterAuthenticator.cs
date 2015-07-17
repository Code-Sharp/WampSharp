using System.Collections.Generic;
using System.Runtime.Serialization;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public interface IWampAuthorizer
    {
        bool CanRegister(RegisterOptions options, string procedure);
        bool CanCall(CallOptions options, string procedure);
        bool CanPublish(PublishOptions options, string topicUri);
        bool CanSubscribe(SubscribeOptions options, string topicUri);
    }

    public interface IWampAuthenticatedConnection<TMessage> : IWampConnection<TMessage>
    {
        IWampSessionAuthenticator Authenticator { get; }
    }

    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Welcome)]
    public class WelcomeDetails : WampDetailsOptions
    {
        [DataMember(Name = "authrole")]
        public string AuthenticationRole { get; set; }

        [DataMember(Name = "authmethod")]
        public string AuthenticationMethod { get; internal set; }

        [DataMember(Name = "authprovider")]
        public string AuthenticationProvider { get; set; }

        [DataMember(Name = "roles")]
        public RouterRoles Roles { get; internal set; }

        [DataMember(Name = "authid")]
        public string AuthenticationId { get; internal set; }
    }

    public interface IWampSessionAuthenticator
    {
        bool IsAuthenticated { get; }

        string AuthenticationId { get; }

        string AuthenticationMethod { get; }

        ChallengeDetails ChallengeDetails { get; }

        void Authenticate(string signature, AuthenticateExtraData extra);

        IWampAuthorizer Authorizer { get; }

        WelcomeDetails WelcomeDetails { get; }
    }

    public interface IWampSessionAuthenticatorFactory
    {
        IWampSessionAuthenticator GetSessionAuthenticator
            (string realm,
             HelloDetails details,
             IWampSessionAuthenticator transportAuthenticator);
    }

    [WampDetailsOptions(WampMessageType.v2Authenticate)]
    public class AuthenticateExtraData : WampDetailsOptions
    {
    }
}