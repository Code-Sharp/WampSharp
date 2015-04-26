using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Subscribe)]
    public class SubscribeOptions : WampDetailsOptions
    {
        [DataMember(Name = "match")]
        public string Match { get; set; }

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