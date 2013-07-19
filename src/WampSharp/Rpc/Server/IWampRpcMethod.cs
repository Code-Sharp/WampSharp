using System;
using System.Threading.Tasks;

namespace WampSharp.Rpc.Server
{
    public interface IWampRpcMethod
    {
        string Name { get; }

        string ProcUri { get; }

        
        Task<object> InvokeAsync(object[] parameters);

        Type[] Parameters { get; }

        object Invoke(object[] parameters);
    }
}