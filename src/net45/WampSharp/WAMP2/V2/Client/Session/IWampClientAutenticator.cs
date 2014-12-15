using System;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp
{
	public interface IWampClientAutenticator
	{
		ChallengeResult Authenticate(string challenge, ChallengeDetails extra);
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
}