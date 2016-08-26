using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;

namespace WampSharp.V2.Rpc
{
    internal abstract class WampResultExtractor : IWampResultExtractor
    {
        private static readonly object[] mEmptyResult = new object[0];

        public virtual object[] GetArguments(object result)
        {
            return mEmptyResult;
        }

        public virtual IDictionary<string, object> GetArgumentKeywords(object result)
        {
            return null;
        }

        public static IWampResultExtractor GetResultExtractor(LocalRpcOperation operation)
        {
            IWampResultExtractor extractor = new EmptyResultExtractor();

            if (operation.HasResult)
            {
                if (operation.CollectionResultTreatment == CollectionResultTreatment.SingleValue)
                {
                    extractor = new SingleResultExtractor();
                }
                else
                {
                    extractor = new MultiResultExtractor();
                }
            }

            return extractor;
        }

        public static IWampResultExtractor GetValueTupleResultExtractor(MethodInfo method)
        {
            Type tupleType = TaskExtensions.UnwrapReturnType(method.ReturnType);

            IWampResultExtractor result = new PositionalTupleExtractor(tupleType);

            if (method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute)))
            {
                TupleElementNamesAttribute attribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                int valueTupleLength = tupleType.GetValueTupleLength();

                // If the tuple is named, return a named tuple extractor
                if (attribute.TransformNames.Take(valueTupleLength).All(x => x != null))
                {
                    result = new NamedTupleExtractor(tupleType, attribute.TransformNames);
                }
            }

            return result;
        }
    }
}