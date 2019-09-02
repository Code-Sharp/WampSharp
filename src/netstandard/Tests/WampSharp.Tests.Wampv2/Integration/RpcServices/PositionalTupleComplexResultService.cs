using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class PositionalTupleComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
        public (int, int) AddComplex(int a, int ai, int b, int bi)
        {
            return (a + b, ai + bi);
        }
    }
}