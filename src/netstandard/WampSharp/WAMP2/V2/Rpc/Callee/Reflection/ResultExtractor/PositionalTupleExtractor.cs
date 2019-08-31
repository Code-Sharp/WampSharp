using System;
using WampSharp.Core.Utilities.ValueTuple;

namespace WampSharp.V2.Rpc
{
    internal class PositionalTupleExtractor : WampResultExtractor
    {
        private readonly ValueTupleArrayConverter mConverter;

        public PositionalTupleExtractor(Type tupleType)
        {
            mConverter = ValueTupleArrayConverter.Get(tupleType);
        }

        public override object[] GetArguments(object result)
        {
            return mConverter.ToArray(result);
        }
    }
}