using System;
using System.Runtime.Serialization;

namespace WampSharp.V2.Reflection
{
    public class RegistrationDetails
    {
        [DataMember(Name = "uri")]
        public string Uri { get; set; }

        [DataMember(Name = "invoke")]
        public string Invoke { get; set; }

        [DataMember(Name = "id")]
        public int RegistrationId { get; set; }

        [DataMember(Name = "match")]
        public string Match { get; set; }

        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
    }
}