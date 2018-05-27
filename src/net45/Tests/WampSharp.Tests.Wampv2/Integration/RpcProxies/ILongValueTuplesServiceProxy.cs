#if !NET40

using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcProxies
{
    public interface ILongValueTuplesServiceProxy
    {
#if VALUETUPLE
        [WampProcedure("com.myapp.get_long_positional_tuple")]
        (string, string, string, string, string, string, string, string, string, string, int) GetLongPositionalTuple(string name);

        [WampProcedure("com.myapp.get_long_keyword_tuple")]
        (string item1, string item2, string item3, string item4, string item5, string item6, string item7, string item8, int length, string item9, string item10) GetLongKeywordTuple(string name);
#else
        [WampProcedure("com.myapp.get_long_positional_tuple")]
        ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, string, string, int>> GetLongPositionalTuple(string name);

        [WampProcedure("com.myapp.get_long_keyword_tuple")]
        [return: TupleElementNames(new string[]
        {
            "item1",
            "item2",
            "item3",
            "item4",
            "item5",
            "item6",
            "item7",
            "item8",
            "length",
            "item9",
            "item10",
            null,
            null,
            null,
            null
        })]
        ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, int, string, string>> GetLongKeywordTuple(string name);
#endif
    }
}

#endif