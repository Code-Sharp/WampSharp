using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface INamedTupleComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
		[return: TupleElementNames(new string[]
		{
			"c",
			"ci"
		})]
		ValueTuple<int, int> AddComplex(int a, int ai, int b, int bi);
        //(int c, int ci) AddComplex(int a, int ai, int b, int bi);
    }
}