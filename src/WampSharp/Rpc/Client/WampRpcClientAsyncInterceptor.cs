using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace WampSharp.Rpc.Client
{
    internal class WampRpcClientAsyncInterceptor : WampRpcClientInterceptor
    {

        public WampRpcClientAsyncInterceptor(IWampRpcSerializer serializer, IWampRpcClientHandler clientHandler) : base(serializer, clientHandler)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            WampRpcCall<object> call =
                Serializer.Serialize(invocation.Method, invocation.Arguments);

            Task<object> task = ClientHandler.HandleAsync(call);

            Task convertedTask = task.Cast(call.ReturnType);

            invocation.ReturnValue = convertedTask;
        }
    }
}