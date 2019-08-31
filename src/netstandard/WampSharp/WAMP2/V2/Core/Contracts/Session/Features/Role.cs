using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    public class Role<TFeatures>
    {
        [DataMember(Name = "features")]
        public TFeatures Features { get; set; }

        public static implicit operator Role<TFeatures>(TFeatures features)
        {
            return new Role<TFeatures>() {Features = features};
        }
    }
}