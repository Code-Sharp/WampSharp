using System;
using System.Threading.Tasks;
using WampSharp.Rpc.Server;

namespace WampSharp.Tests.Rpc.Helpers
{
    public class MockRpcMethod : IWampRpcMethod
    {
        public Exception Error { get; set; }

        public object Result { get; set; }

        public string Name { get; set; }

        public string ProcUri { get; set; }
        
        public Task<object> InvokeAsync(object[] parameters)
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
        
        public object Invoke(object[] parameters)
        {
            return InvokeAsync(parameters).Result;
        }
    }
}