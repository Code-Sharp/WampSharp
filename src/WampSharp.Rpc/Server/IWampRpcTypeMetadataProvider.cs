using System;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcTypeMetadataProvider
    {
        IWampRpcMetadata Provide(Type type);
    }
}