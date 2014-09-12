using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="IWampEvent"/>.
    /// </summary>
    public class WampEvent : IWampEvent
    {
        public PublishOptions Options { get; set; }
        public object[] Arguments { get; set; }
        public IDictionary<string, object> ArgumentsKeywords { get; set; }
    }
}