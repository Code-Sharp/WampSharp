using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class RegistrationDetails
    {
        [PropertyName("uri")]
        public string Uri { get; set; }

        [PropertyName("invoke")]
        public string Invoke { get; set; }

        [PropertyName("id")]
        public int RegistrationId { get; set; }

        [PropertyName("match")]
        public string Match { get; set; }

        [PropertyName("created")]
        public DateTime Created { get; set; }
    }
}