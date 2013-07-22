using System;
using System.Reflection;

namespace WampSharp.Rpc.Client
{
    public class WampRpcMethodAttributeProcUriMapper : IWampProcUriMapper
    {
        public string Map(MethodInfo method)
        {
            WampRpcMethodAttribute rpcMethodAttribute =
                method.GetCustomAttribute<WampRpcMethodAttribute>(true);

            if (rpcMethodAttribute == null)
            {
                throw new ArgumentException("Method doesn't have WampRpcMethodAttribute", "method");
            }

            return rpcMethodAttribute.ProcUri;
        }
    }
}