﻿#if !NET40

using System;
using System.Runtime.CompilerServices;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration.RpcServices
{
#if VALUETUPLE
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
#else
    public class LongValueTuplesCalleeService
    {
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
        public ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, int, string, string>> GetLongKeywordTuple(string name)
        {
            return new ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, int, string, string>>(name + " 1", name + " 2", name + " 3", name + " 4", name + " 5", name + " 6", name + " 7", new ValueTuple<string, int, string, string>(name + " 8", name.Length, name + " 9", name + " 10"));
        }

        [WampProcedure("com.myapp.get_long_positional_tuple")]
        public ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, string, string, int>> GetLongPositionalTuple(string name)
        {
            return new ValueTuple<string, string, string, string, string, string, string, ValueTuple<string, string, string, int>>(name + " 1", name + " 2", name + " 3", name + " 4", name + " 5", name + " 6", name + " 7", new ValueTuple<string, string, string, int>(name + " 8", name + " 9", name + " 10", name.Length));
        }
    }
#endif
}

#endif