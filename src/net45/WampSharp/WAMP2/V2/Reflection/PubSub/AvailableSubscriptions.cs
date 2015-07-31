using System.Runtime.Serialization;

namespace WampSharp.V2.Reflection
{
    public class AvailableSubscriptions
    {
        [DataMember(Name = "prefix")]
        public long[] Prefix { get; set; }
        
        [DataMember(Name = "exact")]
        public long[] Exact { get; set; }
        
        [DataMember(Name = "wildcard")]
        public long[] Wildcard { get; set; }
    }
}