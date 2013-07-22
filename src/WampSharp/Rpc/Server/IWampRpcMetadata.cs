using System.Reflection;
using System.Collections.Generic;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcMetadata
    {
        IEnumerable<IWampRpcMethod> GetServiceMethods();
    }
}