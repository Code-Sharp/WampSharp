using System;
using System.Collections.Generic;
using System.Linq;

namespace WampSharp.Rpc.Server
{
    public class MethodInfoWampRpcMetadata : IWampRpcMetadata
    {
        private readonly Type mType;

        public MethodInfoWampRpcMetadata(Type type)
        {
            mType = type;
        }

        public IEnumerable<IWampRpcMethod> GetServiceMethods()
        {
            return mType.GetMethods()
                        .Where(method => method.IsDefined(typeof(WampRpcMethodAttribute), true))
                        .Select(method => new MethodInfoWampRpcMethod(method));
        }
    }
}