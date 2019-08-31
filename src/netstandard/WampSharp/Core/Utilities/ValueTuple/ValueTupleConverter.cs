using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal abstract class ValueTupleConverter
    {
        private const int MaxTupleSize = 7;
        private readonly Type mTupleType;

        protected ValueTupleConverter(Type tupleType)
        {
            ThrowHelper.ValidateSimpleTuple(tupleType);
            mTupleType = tupleType;
        }

        protected Func<TSource, object> BuildToTuple<TSource>(ParameterExpression sourceParameter, IEnumerable<Expression> tupleItemValuesExpressions)
        {
            NewExpression tupleCreation = 
                GetTupleNewExpression(tupleItemValuesExpressions, mTupleType);

            // new ValueTuple<T1, T2, ..>((T1)dictionary["$argument1Name"],(T2)dictionary["$argument2Name"], ...)

            UnaryExpression boxed =
                Expression.Convert(tupleCreation, typeof(object));

            var lambda = Expression.Lambda<Func<TSource, object>>
                (boxed, sourceParameter);

            var compiled = lambda.Compile();

            return compiled;
        }

        public static NewExpression GetTupleNewExpression(IEnumerable<Expression> tupleItemValuesExpressions, Type tupleType)
        {
            Type[] genericArguments = tupleType.GetGenericArguments();

            IEnumerable<Expression> tupleSimpleArguments = tupleItemValuesExpressions.Zip
                (genericArguments.Take(MaxTupleSize),
                 (expression, type) =>
                     Expression.Convert(expression,
                                        type));

            IEnumerable<Expression> tupleConstructorArguments = tupleSimpleArguments;

            if (genericArguments.Length > MaxTupleSize)
            {
                IEnumerable<Expression> nestedItemValueExpressions = 
                    tupleItemValuesExpressions.Skip(MaxTupleSize);

                Type nestedTupleType = genericArguments.Last();

                NewExpression newExpression = 
                    GetTupleNewExpression(nestedItemValueExpressions, nestedTupleType);

                tupleConstructorArguments = tupleConstructorArguments.Concat(new[] {newExpression});
            }

            NewExpression tupleCreation =
                Expression.New(tupleType.GetConstructors().FirstOrDefault(),
                               tupleConstructorArguments);

            return tupleCreation;
        }

        public static IEnumerable<Expression> GetTupleItemsExpressions
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
            IEnumerable<Expression> simpleFields =
                tupleType.GetFields()
                         .Where(x => x.Name.StartsWith("Item"))
                         .OrderBy(x => x.Name)
                         .Select(field => Expression.Field(tupleInstance, field));

            IEnumerable<Expression> result = simpleFields;

            Type[] genericArguments = tupleType.GetGenericArguments();

            if (genericArguments.Length > MaxTupleSize)
            {
                FieldInfo restField = tupleType.GetField("Rest");

                MemberExpression nestedTupleInstance = 
                    Expression.Field(tupleInstance, restField);

                Type nestedTupleType = genericArguments.Last();

                IEnumerable<Expression> nestedFields = 
                    InnerGetTupleItemsExpressions(nestedTupleInstance, nestedTupleType);

                result = result.Concat(nestedFields);
            }

            return result;
        }

        protected static class ThrowHelper
        {
            public static void ValidateSimpleTuple(Type tupleType)
            {
                if (!tupleType.IsValueTuple())
                {
                    throw new ArgumentException("Expected a ValueTuple type", nameof(tupleType));
                }
            }
        }
    }
}