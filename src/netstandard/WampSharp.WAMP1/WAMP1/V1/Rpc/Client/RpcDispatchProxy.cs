using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V1.Rpc.Client
{
    public class RpcDispatchProxy : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            WampRpcClientInterceptor interceptor;
 
            if (typeof(Task).IsAssignableFrom(targetMethod.ReturnType))
            {
                interceptor = AsyncInterceptor;
            }
            else
            {
                interceptor = SyncInterceptor;
            }

            object result = interceptor.Invoke(targetMethod, args);

            return result;
        }

        internal WampRpcClientSyncInterceptor SyncInterceptor {get; set; }

        internal WampRpcClientAsyncInterceptor AsyncInterceptor {get; set; }
    }
}