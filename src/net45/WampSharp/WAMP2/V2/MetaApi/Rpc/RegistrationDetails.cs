using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    [DataContract]
    public class RegistrationDetails
    {
        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [DataMember(Name = "invoke")]
        public string Invoke { get; set; }

        [DataMember(Name = "id")]
        public long RegistrationId { get; set; }

        [DataMember(Name = "match")]
        public string Match { get; set; }

        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
    }
}