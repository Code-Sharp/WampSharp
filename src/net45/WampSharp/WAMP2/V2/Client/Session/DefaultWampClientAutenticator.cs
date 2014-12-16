using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WampSharp.V2.Client
{
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
            throw new NotImplementedException("Authorization was requested but no authenticator was provided");
        }
    }
}
