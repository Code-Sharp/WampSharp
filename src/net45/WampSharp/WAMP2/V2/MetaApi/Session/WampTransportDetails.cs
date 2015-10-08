using System.Runtime.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.MetaApi
{
    [DataContract]
    public class WampTransportDetails : WampDetailsOptions
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}