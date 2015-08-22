using System.Runtime.Serialization;

namespace WampSharp.V2.Reflection
{
    [DataContract]
    public class AvailableSubscriptions
    {
        private static long[] mEmptyArray = new long[0];

        public AvailableSubscriptions()
        {
            Exact = mEmptyArray;
            Prefix = mEmptyArray;
            Wildcard = mEmptyArray;
        }

        [DataMember(Name = "exact")]
        public long[] Exact { get; set; }

        [DataMember(Name = "prefix")]
        public long[] Prefix { get; set; }

        [DataMember(Name = "wildcard")]
        public long[] Wildcard { get; set; }
    }
}