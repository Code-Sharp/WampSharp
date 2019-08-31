using CliFx.Attributes;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.Callee
{
    [Command("complex")]
    public class ComplexCommand : CalleeCommand<ComplexResultService>
    {
    }

    public interface IComplexResultService
    {
        [WampProcedure("com.myapp.add_complex")]
        (int c, int ci) AddComplex(int a, int ai, int b, int bi);

        [WampProcedure("com.myapp.split_name")]
        (string, string) SplitName(string fullname);
    }

    public class ComplexResultService : IComplexResultService
    {
        public (int c, int ci) AddComplex(int a, int ai, int b, int bi)
        {
            return (a + b, ai + bi);
        }

        public (string, string) SplitName(string fullname)
        {
            string[] splitted = fullname.Split(' ');

            string forename = splitted[0];
            string surname = splitted[1];

            return (forename, surname);
        }
    }
}