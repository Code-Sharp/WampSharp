using System;
using System.Collections.Generic;
using WampSharp.Core.Utilities.ValueTuple;

namespace WampSharp.V2.Rpc
{
    internal class NamedTupleExtractor : WampResultExtractor
    {
        private readonly ValueTupleDictionaryConverter mConverter;

        public NamedTupleExtractor(Type tupleType, IList<string> transformNames)
        {
            mConverter = new ValueTupleDictionaryConverter(transformNames, tupleType);
        }

        public override IDictionary<string, object> GetArgumentKeywords(object result)
        {
            return mConverter.ToDictionary(result);
        }
    }
}