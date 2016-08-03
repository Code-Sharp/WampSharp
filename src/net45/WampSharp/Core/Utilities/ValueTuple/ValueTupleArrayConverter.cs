using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal class ValueTupleArrayConverter : ValueTupleConverter
    {
        private readonly Type mTupleType;

        private readonly Func<object, object[]> mToArrayDelegate;

        private readonly Func<object[], object> mToTupleDelegate;

        public ValueTupleArrayConverter(Type tupleType) : base(tupleType)
        {
            mTupleType = tupleType;

            mToArrayDelegate = BuildToArray();
            mToTupleDelegate = BuildToTuple();
        }

        public object ToTuple(object[] array)
        {
            return mToTupleDelegate(array);
        }

        public object[] ToArray(object valueTuple)
        {
            return mToArrayDelegate(valueTuple);
        }

        private Func<object[], object> BuildToTuple()
        {
            ParameterExpression array =
                Expression.Parameter(typeof(object[]), "array");

            IEnumerable<BinaryExpression> tupleItemValues =
                Enumerable.Range(0, mTupleType.GetValueTupleLength())
                          .Select(index => Expression.ArrayIndex(array, Expression.Constant(index)));

            // new ValueTuple<T1, T2, ..>((T1)array[0],(T2)array[1], ...)

            Func<object[], object> result =
                BuildToTuple<object[]>
                    (array, tupleItemValues);

            return result;
        }

        private Func<object, object[]> BuildToArray()
        {
            ParameterExpression tupleResult =
                Expression.Parameter(typeof(object), "tupleResult");

            ParameterExpression casted =
                Expression.Variable(mTupleType, "casted");

            BinaryExpression assignment =
                Expression.Assign(casted, Expression.Convert(tupleResult, mTupleType));

            IEnumerable<Expression> parameters =
                GetTupleItemsExpressions(casted, mTupleType);

            NewArrayExpression arrayInit =
                Expression.NewArrayInit(typeof(object), parameters);

            BlockExpression body =
                Expression.Block(variables: new[] { casted },
                                 expressions: new Expression[]
                                 {
                                     assignment,
                                     arrayInit
                                 });

            Expression<Func<object, object[]>> lambda =
                Expression.Lambda<Func<object, object[]>>
                    (body: body,
                     parameters: tupleResult);

            Func<object, object[]> result = lambda.Compile();

            return result;
        }
    }
}