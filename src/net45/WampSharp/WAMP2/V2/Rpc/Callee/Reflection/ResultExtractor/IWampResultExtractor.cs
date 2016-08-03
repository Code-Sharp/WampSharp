using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;

namespace WampSharp.V2.Rpc
{
    internal interface IWampResultExtractor
    {
        object[] GetArguments(object result);
        IDictionary<string, object> GetArgumentKeywords(object result);
    }

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

            if (!method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute)))
            {
                return new PositionalTupleExtractor(tupleType);
            }
            else
            {
                TupleElementNamesAttribute attribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                return new NamedTupleExtractor(tupleType, attribute.TransformNames);
            }
        }
    }

    internal class SingleResultExtractor : WampResultExtractor
    {
        public override object[] GetArguments(object result)
        {
            return new object[] {result};
        }
    }

    internal class EmptyResultExtractor : WampResultExtractor
    {
    }

    internal class MultiResultExtractor : WampResultExtractor
    {
        public override object[] GetArguments(object result)
        {
            return GetFlattenResult((dynamic) result);
        }

        private object[] GetFlattenResult<T>(ICollection<T> result)
        {
            return result.Cast<object>().ToArray();
        }

        private object[] GetFlattenResult(object result)
        {
            return new object[] {result};
        }
    }

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

    internal class PositionalTupleExtractor : WampResultExtractor
    {
        private readonly ValueTupleArrayConverter mConverter;

        public PositionalTupleExtractor(Type tupleType)
        {
            mConverter = new ValueTupleArrayConverter(tupleType);
        }

        public override object[] GetArguments(object result)
        {
            return mConverter.ToArray(result);
        }
    }
}