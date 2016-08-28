#if !NET40
using System;
using System.Runtime.CompilerServices;
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
		[return: TupleElementNames(new string[]
		{
			"c",
			"ci"
		})]
		ValueTuple<int, int> AddComplex(int a, int ai, int b, int bi);

        //Task<(int c, int ci)> AddComplexAsync(int a, int ai, int b, int bi);
        [WampProcedure("com.myapp.add_complex")]
        [return: TupleElementNames(new string[]
        {
            "c",
            "ci"
        })]
        Task<ValueTuple<int, int>> AddComplexAsync(int a, int ai, int b, int bi);
    }
}

#endif