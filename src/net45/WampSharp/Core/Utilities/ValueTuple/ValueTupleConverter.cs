using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal abstract class ValueTupleConverter
    {
        private readonly Type mTupleType;

        protected ValueTupleConverter(Type tupleType)
        {
            ThrowHelper.ValidateSimpleTuple(tupleType);
            mTupleType = tupleType;
        }

        protected Func<TSource, object> BuildToTuple<TSource>(ParameterExpression sourceParameter, IEnumerable<Expression> tupleItemValuesExpressions)
        {
            NewExpression tupleCreation =
                Expression.New(mTupleType.GetConstructors().FirstOrDefault(),
                               tupleItemValuesExpressions.Zip
                                   (mTupleType.GetGenericArguments(),
                                    (expression, type) =>
                                        Expression.Convert(expression,
                                                           type)));

            // new ValueTuple<T1, T2, ..>((T1)dictionary["$argument1Name"],(T2)dictionary["$argument2Name"], ...)

            UnaryExpression boxed =
                Expression.Convert(tupleCreation, typeof(object));

            var lambda = Expression.Lambda<Func<TSource, object>>
                (boxed, sourceParameter);

            var compiled = lambda.Compile();

            return compiled;
        }

        protected static IEnumerable<Expression> GetTupleItemsExpressions
            (Expression tupleInstance,
             Type tupleType)
        {
            IEnumerable<Expression> result =
                InnerGetTupleItemsExpressions(tupleInstance, tupleType);

            return result.Select(x => Expression.Convert(x, typeof(object)));
        }

        protected static IEnumerable<Expression> InnerGetTupleItemsExpressions
            (Expression tupleInstance,
             Type tupleType)
        {
            return tupleType.GetFields()
                            .Where(x => x.Name.StartsWith("Item"))
                            .OrderBy(x => x.Name)
                            .Select(field => Expression.Field(tupleInstance, field));
        }

        protected static class ThrowHelper
        {
            public static void ValidateSimpleTuple(Type tupleType)
            {
                if (!tupleType.IsValueTuple())
                {
                    throw new ArgumentException("Expected a ValueTuple type", "tupleType");
                }

                if (tupleType.GetValueTupleLength() > 7)
                {
                    throw new ArgumentException("Expected a simple ValueTuple (of length at most 7)", "tupleType");
                }

                if (tupleType.GetGenericArguments().Any(x => ValueTupleTypeExtensions.IsValueTuple(x)))
                {
                    throw new ArgumentException("Expected a simple ValueTuple (with no nested ValueTuples)", "tupleType");
                }
            }
        }
    }
}