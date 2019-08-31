using System.Reflection;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1.Cra
{
    internal class MethodInfoWampCraRpcMethod : MethodInfoWampRpcMethod
    {
        public MethodInfoWampCraRpcMethod(MethodInfo method, string baseUri):base(null,method,baseUri)
        {

        }

        protected override object GetInstance(IWampClient client)
        {
            return client.CraAuthenticator;
        }
    }
}