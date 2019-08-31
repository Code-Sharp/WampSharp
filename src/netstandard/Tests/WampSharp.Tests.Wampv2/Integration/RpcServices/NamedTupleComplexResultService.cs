#if !NET40
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class NamedTupleComplexResultService
    {
#if VALUETUPLE
        [WampProcedure("com.myapp.add_complex")]
        public (int c, int ci) AddComplex(int a, int ai, int b, int bi)
        {
            return (a + b, ai + bi);
        }
#else
        [WampProcedure("com.myapp.add_complex")]
        [return: TupleElementNames(new string[]
        {
            "c",
            "ci"
        })]
        public ValueTuple<int, int> AddComplex(int a, int ai, int b, int bi)
        {
            return new ValueTuple<int, int>(a + b, ai + bi);
        }
#endif
    }
}
#endif