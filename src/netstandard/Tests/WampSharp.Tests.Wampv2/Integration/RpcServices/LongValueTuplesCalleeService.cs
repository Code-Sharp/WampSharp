using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
    public class LongValueTuplesCalleeService
    {
        [WampProcedure("com.myapp.get_long_keyword_tuple")]
        public (string item1, string item2, string item3, string item4, string item5, string item6, string item7, string item8, int length, string item9, string item10) GetLongKeywordTuple(string name)
        {
            return (name + " 1", name + " 2", name + " 3", name + " 4", name + " 5", name + " 6", name + " 7", name + " 8", name.Length, name + " 9", name + " 10");
        }

        [WampProcedure("com.myapp.get_long_positional_tuple")]
        public (string, string, string, string, string, string, string, string, string, string, int) GetLongPositionalTuple(string name)
        {
            return (name + " 1", name + " 2", name + " 3", name + " 4", name + " 5", name + " 6", name + " 7", name + " 8", name + " 9", name + " 10", name.Length);
        }
    }
}