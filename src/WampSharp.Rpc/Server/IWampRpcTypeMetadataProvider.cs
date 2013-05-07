using System;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcTypeMetadataProvider
    {
        IWampRpcMetadata Provide(Type type);
    }

    class WampRpcTypeMetadataProvider : IWampRpcTypeMetadataProvider
    {
        public IWampRpcMetadata Provide(Type type)
        {
            return new MethodInfoWampRpcMetadata(type);
        }
    }
}