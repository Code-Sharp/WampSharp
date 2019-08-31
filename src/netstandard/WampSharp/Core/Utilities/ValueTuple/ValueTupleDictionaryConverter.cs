using System;
using System.Collections.Generic;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal class ValueTupleDictionaryConverter : ValueTupleConverter
    {
        private readonly Func<object, IDictionary<string, object>> mToDictionaryDelegate;

        private readonly Func<IDictionary<string, object>, object> mToTupleDelegate;

        public ValueTupleDictionaryConverter(Type tupleType, Func<object, IDictionary<string, object>> toDictionaryDelegate, Func<IDictionary<string, object>, object> toTupleDelegate) :
            base(tupleType)
        {
            Validate(tupleType);
            mToDictionaryDelegate = toDictionaryDelegate;
            mToTupleDelegate = toTupleDelegate;
        }

        private void Validate(Type tupleType)
        {
            ThrowHelper.ValidateSimpleTuple(tupleType);
        }

        public object ToTuple(IDictionary<string, object> dictionary)
        {
            return mToTupleDelegate(dictionary);
        }

        public IDictionary<string, object> ToDictionary(object valueTuple)
        {
            return mToDictionaryDelegate(valueTuple);
        }
    }
}