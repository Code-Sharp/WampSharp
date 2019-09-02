using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface ILongValueTuplesServiceProxy
    {
        [WampProcedure("com.myapp.get_long_positional_tuple")]
        (string, string, string, string, string, string, string, string, string, string, int) GetLongPositionalTuple(string name);

        [WampProcedure("com.myapp.get_long_keyword_tuple")]
        (string item1, string item2, string item3, string item4, string item5, string item6, string item7, string item8, int length, string item9, string item10) GetLongKeywordTuple(string name);
    }
}