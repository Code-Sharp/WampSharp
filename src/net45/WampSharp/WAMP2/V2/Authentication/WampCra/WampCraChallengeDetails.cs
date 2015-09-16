using System.Runtime.Serialization;
using WampSharp.Core.Cra;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    [DataContract]
    public class WampCraChallengeDetails : ChallengeDetails, IWampCraChallenge
    {
        private const int DefaultIterations = 1000;
        private const int DefaultKeyLength = 32;

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