using System.Runtime.Serialization;
using WampSharp.Core.Cra;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An implementation of <see cref="ChallengeDetails"/> specific for WAMP-CRA authentication.
    /// </summary>
    [DataContract]
    public class WampCraChallengeDetails : ChallengeDetails, IWampCraChallenge
    {
        private const int DefaultIterations = 1000;
        private const int DefaultKeyLength = 32;

        /// <summary>
        /// Instantiates a new instance of <see cref="WampCraChallengeDetails"/>.
        /// </summary>
        /// <param name="salt">The salt to use, sent to the user upon CHALLENGE.extra.</param>
        /// <param name="iterations">The number of iterations to use, sent to the user upon CHALLENGE.extra.</param>
        /// <param name="keyLen">The key length to use, sent to the user upon CHALLENGE.extra.</param>
        public WampCraChallengeDetails(string salt,
                                       int? iterations = DefaultIterations,
                                       int? keyLen = DefaultKeyLength)
        {
            Salt = salt;
            Iterations = iterations ?? DefaultIterations;
            KeyLength = keyLen ?? DefaultKeyLength;
        }

        internal WampCraChallengeDetails()
        {
        }

        /// <summary>
        /// Gets the authentication challenge - this is the challenge that will be sent upon CHALLENGE
        /// message. (in challenge.extra)
        /// </summary>
        [DataMember(Name = "challenge")]
        public string Challenge { get; internal set; }

        [DataMember(Name = "salt")]
        public string Salt { get; private set; }

        [DataMember(Name = "iterations")]
        public int? Iterations { get; private set; }

        [DataMember(Name = "keylen")]
        public int? KeyLength { get; private set; }
    }
}