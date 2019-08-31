using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class ComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
        public void AddComplex(int a, int ai, int b, int bi, out int c, out int ci)
        {
            c = a + b;
            ci = ai + bi;
        }

        [WampProcedure("com.myapp.split_name")]
        [return: WampResult(CollectionResultTreatment.Multivalued)]
        public string[] SplitName(string fullname)
        {
            string[] splitted = fullname.Split(' ');
            // Not a complex type but I think thats enough for now.
            return splitted;
        }
    }
}