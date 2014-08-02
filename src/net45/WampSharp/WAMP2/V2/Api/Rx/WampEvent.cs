using System.Collections.Generic;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="IWampEvent"/>.
    /// </summary>
    public class WampEvent : IWampEvent
    {
        public IDictionary<string, object> Options { get; set; }
        public object[] Arguments { get; set; }
        public IDictionary<string, object> ArgumentsKeywords { get; set; }
    }
}