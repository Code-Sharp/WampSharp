using System.Collections.Generic;

namespace WampSharp.Core.Proxy
{
    public interface IWampClientContext
    {
        IDictionary<string, object> ClientContext { get; }
    }
}
