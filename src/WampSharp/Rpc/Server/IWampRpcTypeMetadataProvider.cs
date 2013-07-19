using System;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcTypeMetadataProvider
    {
        IWampRpcMetadata Provide(object instance);
    }
}