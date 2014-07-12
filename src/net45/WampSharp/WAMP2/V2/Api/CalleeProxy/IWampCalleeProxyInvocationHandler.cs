using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyInvocationHandler
    {
        object Invoke(MethodInfo method, object[] arguments);
        Task InvokeAsync(MethodInfo method, object[] arguments);
    }
}