using System;
using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [Serializable]
    [WampDetailsOptions(WampMessageType.v2Subscribe)]
    public class SubscribeOptions : WampDetailsOptions
    {
        public SubscribeOptions()
        {
        }

        public SubscribeOptions(SubscribeOptions options)
        {
            Match = options.Match;
            GetRetained = options.GetRetained;
        }

        /// <summary>
        /// The topic matching method to be used for the subscription.
        /// (Mostly supported: <see cref="WampMatchPattern"/> values: null/"exact"/"prefix"/"wildcard")
        /// </summary>
        [DataMember(Name = "match")]
        public string Match { get; set; }

        /// <summary>
        /// Returns a value indicating whether the client wants the retained message we may have along with the subscription.
        /// </summary>
        [DataMember(Name = "get_retained")]
        public bool? GetRetained { get; set; }

        protected bool Equals(SubscribeOptions other)
        {
            return string.Equals(Match, other.Match);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SubscribeOptions) obj);
        }

        public override int GetHashCode()
        {
            return (Match != null ? Match.GetHashCode() : 0);
        }
    }
}