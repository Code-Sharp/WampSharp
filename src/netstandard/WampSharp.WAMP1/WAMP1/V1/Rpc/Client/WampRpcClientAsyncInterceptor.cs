using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;

namespace WampSharp.V1.Rpc.Client
{
    internal class WampRpcClientAsyncInterceptor : WampRpcClientInterceptor
    {

        public WampRpcClientAsyncInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler) : base(serializer, clientHandler)
        {
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            WampRpcCall call =
                Serializer.Serialize(method, arguments);

            Task<object> task = ClientHandler.HandleAsync(call);

            Task convertedTask = task.Cast(call.ReturnType);

            return convertedTask;
        }
    }
}