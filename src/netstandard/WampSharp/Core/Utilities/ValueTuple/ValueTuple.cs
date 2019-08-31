#if NET40

using System;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.ReturnValue)]
    internal sealed class TupleElementNamesAttribute : Attribute
    {
        public IList<string> TransformNames { get; private set; }

        public TupleElementNamesAttribute(string[] transformNames)
        {
            TransformNames = transformNames;
        }
    }
}

namespace System
{
    internal class ValueTuple<T1>
    {
    }

    internal class ValueTuple<T1, T2>
    {
    }

    internal class ValueTuple<T1, T2, T3>
    {
    }

    internal class ValueTuple<T1, T2, T3, T4>
    {
    }

    internal class ValueTuple<T1, T2, T3, T4, T5>
    {
    }

    internal class ValueTuple<T1, T2, T3, T4, T5, T6>
    {
    }

    internal class ValueTuple<T1, T2, T3, T4, T5, T6, T7>
    {
    }

    internal class ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>
    {
    }
}

#endif