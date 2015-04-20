using System;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.CodeGeneration
{
    public interface IArgumentsService
    {
        [WampProcedure("com.arguments.ping")]
        void Ping();

        [WampProcedure("com.arguments.add2")]
        int Add2(int a, int b);

        [WampProcedure("com.arguments.stars")]
        string Stars(string nick = "somebody", int stars = 0);

        [WampProcedure("com.arguments.orders")]
        string[] Orders(string product, int limit = 5);

        [WampProcedure("com.arguments.ping")]
        Task PingAsync();

        [WampProcedure("com.arguments.add2")]
        Task<int> Add2Async(int a, int b);

        [WampProcedure("com.arguments.stars")]
        Task<string> StarsAsync(string nick = "somebody", int stars = 0);

        [WampProcedure("com.arguments.orders")]
        Task<string[]> OrdersAsync(string product, int limit = 5);

        [WampProcedure("com.myapp.longop")]
        [WampProgressiveResultProcedure]
        Task<int> LongOp(int n, IProgress<int> progress);

        [WampProcedure("com.myapp.split_name")]
        [return: WampResult(CollectionResultTreatment.Multivalued)]
        string[] SplitName(string fullname);

        [WampProcedure("com.myapp.split_name")]
        [return: WampResult(CollectionResultTreatment.Multivalued)]
        Task<string[]> SplitNameAsync(string fullname);    
    }
}