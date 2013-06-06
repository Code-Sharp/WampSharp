using System;

namespace WampSharp.Rpc.Server
{
    class WampRpcTypeMetadataProvider : IWampRpcTypeMetadataProvider
    {
        public IWampRpcMetadata Provide(object instance)
        {
            return new MethodInfoWampRpcMetadata(instance);
        }
    }
}