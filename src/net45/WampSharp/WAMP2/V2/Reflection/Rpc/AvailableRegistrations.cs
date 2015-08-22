using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class AvailableRegistrations
    {
        [DataMember(Name = WampMatchPattern.Prefix)]
        public long[] Prefix { get; set; }

        [DataMember(Name = WampMatchPattern.Exact)]
        public long[] Exact { get; set; }

        [DataMember(Name = WampMatchPattern.Wildcard)]
        public long[] Wildcard { get; set; }
    }
}