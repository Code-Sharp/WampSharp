using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface IPositionalTupleComplexResultService
    {
        [WampProcedure("com.myapp.split_name")]
        (string, string) SplitName(string fullname);

        [WampProcedure("com.myapp.split_name")]
        Task<(string, string)> SplitNameAsync(string fullname);
    }

    public interface INamedTupleComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
        (int c, int ci) AddComplex(int a, int ai, int b, int bi);

        [WampProcedure("com.myapp.add_complex")]
        Task<(int c, int ci)> AddComplexAsync(int a, int ai, int b, int bi);
    }
}