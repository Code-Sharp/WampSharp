using System.Reflection;
using WampSharp.Rpc.Server;

namespace WampSharp.Cra
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
