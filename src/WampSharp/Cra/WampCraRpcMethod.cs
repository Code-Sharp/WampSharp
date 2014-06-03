using System.Reflection;
using WampSharp.Core.Contracts.V1;
using WampSharp.Rpc.Server;

namespace WampSharp.Cra
{
    internal class MethodInfoWampCraRpcMethod : MethodInfoWampRpcMethod
    {
        public MethodInfoWampCraRpcMethod(MethodInfo method, string baseUri):base(null,method,baseUri)
        {

        }

        public override object GetInstance(IWampClient client)
        {
            return client.CraAuthenticator;
        }
    }
}