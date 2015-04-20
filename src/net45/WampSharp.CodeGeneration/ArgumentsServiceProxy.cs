using System;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Rpc;

namespace MyNamespace
{
    public class ArgumentsServiceProxy : CalleeProxyBase, global::WampSharp.CodeGeneration.IArgumentsService
    {
        public ArgumentsServiceProxy(IWampChannel channel, ICalleeProxyInterceptor interceptor)
            : base(channel, interceptor)
        {
        }

        [WampProcedure("com.arguments.ping")]
        public void Ping()
        {
            SingleInvokeSync(MethodBase.GetCurrentMethod());
        }

        [WampProcedure("com.arguments.add2")]
        public int Add2(int a, int b)
        {
            return SingleInvokeSync<int>(MethodBase.GetCurrentMethod(), a, b);
        }

        [WampProcedure("com.arguments.stars")]
        public string Stars(string nick, int stars)
        {
            return SingleInvokeSync<string>(MethodBase.GetCurrentMethod(), nick, stars);
        }

        [WampProcedure("com.arguments.orders")]
        public string[] Orders(string product, int limit)
        {
            return SingleInvokeSync<string[]>(MethodBase.GetCurrentMethod(), product, limit);
        }

        [WampProcedure("com.arguments.ping")]
        public Task PingAsync()
        {
            return SingleInvokeAsync(MethodBase.GetCurrentMethod());
        }

        [WampProcedure("com.arguments.add2")]
        public Task<int> Add2Async(int a, int b)
        {
            return SingleInvokeAsync<int>(MethodBase.GetCurrentMethod(), a, b);
        }

        [WampProcedure("com.arguments.stars")]
        public Task<string> StarsAsync(string nick, int stars)
        {
            return SingleInvokeAsync<string>(MethodBase.GetCurrentMethod(), nick, stars);
        }

        [WampProcedure("com.arguments.orders")]
        public Task<string[]> OrdersAsync(string product, int limit)
        {
            return SingleInvokeAsync<string[]>(MethodBase.GetCurrentMethod(), product, limit);
        }

        [WampProcedure("com.myapp.longop")]
        public Task<int> LongOp(int n, IProgress<int> progress)
        {
            return SingleInvokeProgressiveAsync<int>(MethodBase.GetCurrentMethod(), progress, n);
        }

        [WampProcedure("com.myapp.split_name")]
        public string[] SplitName(string fullname)
        {
            return MultiInvokeSync<string>(MethodBase.GetCurrentMethod(), fullname);
        }

        [WampProcedure("com.myapp.split_name")]
        public Task<string[]> SplitNameAsync(string fullname)
        {
            return MultiInvokeAsync<string>(MethodBase.GetCurrentMethod(), fullname);
        }
    }
}