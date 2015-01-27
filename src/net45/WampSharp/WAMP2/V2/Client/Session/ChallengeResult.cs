using System.Collections.Generic;

namespace WampSharp.V2.Client
{
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