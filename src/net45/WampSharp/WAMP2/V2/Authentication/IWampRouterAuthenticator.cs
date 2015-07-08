using WampSharp.Core.Listener;
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

    public interface IWampSessionAuthenticator
    {
        bool IsAuthenticated { get; }

        string AuthenticationId { get; }

        string AuthenticationMethod { get; }
        
        ChallengeDetails Details { get; }
        
        void Authenticate(string signature, AuthenticateExtraData extra);

        IWampAuthorizer Authorizer { get; }
    }

    public interface IWampSessionAuthenticatorFactory
    {
        IWampSessionAuthenticator GetSessionAuthenticator
            (IWampSessionAuthenticator transportAuthenticator,
             string realm,
             HelloDetails details);
    }

    public class AuthenticateExtraData : WampDetailsOptions
    {
    }

    public class ChallengeRequest
    {
        public string AuthenticationMethod { get; set; }

        public ChallengeDetails Details { get; set; }
    }
}