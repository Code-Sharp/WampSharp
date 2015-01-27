using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
	public interface IWampClientAutenticator
	{
		ChallengeResult Authenticate(string challenge, ChallengeDetails extra);

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