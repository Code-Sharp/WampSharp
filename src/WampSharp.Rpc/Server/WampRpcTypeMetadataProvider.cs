using System;

namespace WampSharp.Rpc.Server
{
    class WampRpcTypeMetadataProvider : IWampRpcTypeMetadataProvider
    {
        public IWampRpcMetadata Provide(Type type)
        {
            return new MethodInfoWampRpcMetadata(type);
        }
    }
}