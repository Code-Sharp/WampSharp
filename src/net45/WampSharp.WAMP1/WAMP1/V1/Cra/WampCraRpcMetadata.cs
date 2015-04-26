using System.Reflection;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1.Cra
{
    internal class MethodInfoWampCraRpcMetadata : MethodInfoWampRpcMetadata
    {
        public MethodInfoWampCraRpcMetadata(object instance, string baseUri = null):
            base(instance, baseUri)
        {
        }

        protected override MethodInfoWampRpcMethod CreateRpcMethod(MethodInfo method)
        {
            return new MethodInfoWampCraRpcMethod(method, base.BaseUri);
        }
    }
}
