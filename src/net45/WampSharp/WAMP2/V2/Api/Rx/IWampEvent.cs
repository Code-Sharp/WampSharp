using System.Collections.Generic;

namespace WampSharp.V2
{
    public interface IWampEvent
    {
        IDictionary<string, object> Options { get; }
        object[] Arguments { get; }
        IDictionary<string, object> ArgumentsKeywords { get; }
    }
}