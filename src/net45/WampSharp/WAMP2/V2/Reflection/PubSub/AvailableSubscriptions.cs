using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class AvailableSubscriptions
    {
        [PropertyName("prefix")]
        public long[] Prefix { get; set; }
        
        [PropertyName("exact")]
        public long[] Exact { get; set; }
        
        [PropertyName("wildcard")]
        public long[] Wildcard { get; set; }
    }
}