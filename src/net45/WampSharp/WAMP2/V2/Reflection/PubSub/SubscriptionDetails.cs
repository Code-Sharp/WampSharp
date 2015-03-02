using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class SubscriptionDetails
    {
        [PropertyName("uri")]
        public string Uri { get; set; }

        [PropertyName("id")]
        public long SubscriptionId { get; set; }

        [PropertyName("match")]
        public string Match { get; set; }

        [PropertyName("created")]
        public DateTime Created { get; set; }
    }
}