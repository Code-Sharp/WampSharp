using System.Threading.Tasks;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface IComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
        void AddComplex(int a, int ai, int b, int bi, out int c, out int ci);

        [WampProcedure("com.myapp.split_name")]
        [return: WampResult(CollectionResultTreatment.Multivalued)]
        string[] SplitName(string fullname);

        [WampProcedure("com.myapp.split_name")]
        [return: WampResult(CollectionResultTreatment.Multivalued)]
        Task<string[]> SplitNameAsync(string fullname);
    }
}