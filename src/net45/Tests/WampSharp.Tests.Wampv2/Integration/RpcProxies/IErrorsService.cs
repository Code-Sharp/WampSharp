using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface IErrorsService
    {
        [WampProcedure("com.myapp.sqrt")]
        int Sqrt(int x);

        [WampProcedure("com.myapp.checkname")]
        void CheckName(string name);

        [WampProcedure("com.myapp.compare")]
        void Compare(int a, int b);

        [WampProcedure("com.myapp.sqrt")]
        Task<int> SqrtAsync(int x);

        [WampProcedure("com.myapp.checkname")]
        Task CheckNameAsync(string name);

        [WampProcedure("com.myapp.compare")]
        Task CompareAsync(int a, int b);
    }
}