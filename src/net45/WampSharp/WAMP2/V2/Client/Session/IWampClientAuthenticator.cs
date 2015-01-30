using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
	public interface IWampClientAuthenticator
	{
		AuthenticationResponse Authenticate(string authmethod, ChallengeDetails extra);

        string[] AuthenticationMethods
        {
            get;
        }

        string AuthenticationId
        {
            get;
        }
	}
}