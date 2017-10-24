#if !NET40
using System;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface IPositionalTupleComplexResultService
    {
        [WampProcedure("com.myapp.split_name")]
        ValueTuple<string, string> SplitName(string fullname);

        [WampProcedure("com.myapp.split_name")]
        Task<ValueTuple<string, string>> SplitNameAsync(string fullname);
    }

    public interface INamedTupleComplexResultService
    {
        //(int c, int ci) AddComplex(int a, int ai, int b, int bi);
        [WampProcedure("com.myapp.add_complex")]
        (int c, int ci) AddComplex(int a, int ai, int b, int bi);

        //Task<(int c, int ci)> AddComplexAsync(int a, int ai, int b, int bi);
        [WampProcedure("com.myapp.add_complex")]
        Task<(int c, int ci)> AddComplexAsync(int a, int ai, int b, int bi);
    }
}

#endif
