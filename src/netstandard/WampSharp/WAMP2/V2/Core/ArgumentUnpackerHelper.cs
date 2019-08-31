using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Utilities.ValueTuple;

namespace WampSharp.V2.Core
{
    internal static class ArgumentUnpackerHelper
    {
        public static ArgumentUnpacker GetValueTupleArgumentUnpacker(Type tupleType, IEnumerable<string> tupleElementNames = null)
        {
            bool skipPositionalArguments;

            if (tupleElementNames != null)
            {
                skipPositionalArguments = true;
            }
            else
            {
                tupleElementNames = Enumerable.Repeat(default(string), tupleType.GetValueTupleLength());
                skipPositionalArguments = false;
            }

            var argumentTypes =
                tupleType.GetValueTupleElementTypes()
                         .Select((type, index) => new { type, index });

            IEnumerable<LocalParameter> localParameters =
                tupleElementNames.
                    Zip(argumentTypes,
                        (name, typeToIndex) =>
                            new LocalParameter(name,
                                               typeToIndex.type,
                                               typeToIndex.index));

            ArgumentUnpacker result = new ArgumentUnpacker(localParameters.ToArray());

            result.SkipPositionalArguments = skipPositionalArguments;

            return result;
        }
    }
}