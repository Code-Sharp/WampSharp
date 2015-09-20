using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.MetaApi
{
    [DataContract]
    public class AvailableGroups
    {
        private static readonly long[] mEmptyArray = new long[0];

        public AvailableGroups()
        {
            Exact = mEmptyArray;
            Prefix = mEmptyArray;
            Wildcard = mEmptyArray;
        }

        [DataMember(Name = WampMatchPattern.Exact)]
        public long[] Exact { get; set; }

        [DataMember(Name = WampMatchPattern.Prefix)]
        public long[] Prefix { get; set; }

        [DataMember(Name = WampMatchPattern.Wildcard)]
        public long[] Wildcard { get; set; }
    }
}