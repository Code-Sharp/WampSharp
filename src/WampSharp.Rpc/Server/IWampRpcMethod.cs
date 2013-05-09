using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcMethod
    {
        string Name { get; }

        string ProcUri { get; }

        Task<object> InvokeAsync(object instance, object[] parameters);

        Type[] Parameters { get; }

        object Invoke(object instance, object[] parameters);
    }
}