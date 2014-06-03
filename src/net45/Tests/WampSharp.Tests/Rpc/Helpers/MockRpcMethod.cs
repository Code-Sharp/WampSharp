using System;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V1.Rpc.Server;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockRpcMethod : IWampRpcMethod
    {
        public Exception Error { get; set; }

        public object Result { get; set; }

        public string Name { get; set; }

        public string ProcUri { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public Task<object> InvokeAsync(IWampClient client, object[] parameters)
        {
            TaskCompletionSource<object> result = 
                new TaskCompletionSource<object>();

            if (Error == null)
            {
                result.SetResult(Result);
            }
            else
            {
                result.SetException(Error);
            }

            return result.Task;
        }

        public Type[] Parameters { get; set; }

        public object Invoke(IWampClient client, object[] parameters)
        {
            return InvokeAsync(client, parameters).Result;
        }
    }
}