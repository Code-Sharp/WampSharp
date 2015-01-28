using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyInvocationHandler
    {
        object Invoke(CallOptions options, MethodInfo method, object[] arguments);
        Task InvokeAsync(CallOptions options, MethodInfo method, object[] arguments);
    }
}