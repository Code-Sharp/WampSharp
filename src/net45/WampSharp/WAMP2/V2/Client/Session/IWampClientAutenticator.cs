using System;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp
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
	
	public class ChallengeDetails : WampDetailsOptions
	{
	}
	
	public class ChallengeResult
	{
		public string Signature 
		{
			get;
			set;
		}
		
		public IDictionary<string, object> Extra 
		{
			get;
			set;
		}
	}

    [Serializable]
    public class WampAuthenticationException : Exception
    {
        public WampAuthenticationException() { }

        public WampAuthenticationException(string message) : base(message) { }

        public WampAuthenticationException(string message, Exception inner) : base(message, inner) { }

        protected WampAuthenticationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// A default implementation of <see cref="IWampClientAutenticator"/>.
    /// </summary>
    public class DefaultWampClientAutenticator : IWampClientAutenticator
    {
        /// <summary>
        /// Just throws exception on CHALLENGE
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public ChallengeResult Authenticate(string challenge, ChallengeDetails extra)
        {
            throw new WampAuthenticationException("Authorization was requested but no authenticator was provided");
        }

        public string[] AuthenticationMethods
        {
            get { return null; }
        }

        public string AuthenticationId
        {
            get { return null; }
        }
    }
}