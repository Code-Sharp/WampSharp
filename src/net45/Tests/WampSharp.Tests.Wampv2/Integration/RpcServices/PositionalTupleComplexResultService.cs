#if !NET40

using System;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class PositionalTupleComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
        public ValueTuple<int, int> AddComplex(int a, int ai, int b, int bi)
        {
            return new ValueTuple<int, int>(a + b, ai + bi);
        }
        //public (int, int) AddComplex(int a, int ai, int b, int bi)
        //{
        //    return (a + b, ai + bi);
        //}
    }
}

#endif