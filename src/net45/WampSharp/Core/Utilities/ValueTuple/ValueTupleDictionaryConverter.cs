using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal class ValueTupleDictionaryConverter : ValueTupleConverter
    {
        private readonly IList<string> mTransformNames;
        private readonly Type mTupleType;

        private static readonly MethodInfo mDictionaryAddMethod =
            Method.Get((IDictionary<string, object> dictionary) =>
                           dictionary.Add(default(string), default(object)));

        private static readonly PropertyInfo mDictionaryIndexer =
            typeof(IDictionary<string, object>).GetProperties().
                                                FirstOrDefault(x => x.GetIndexParameters().Any());

        private readonly Func<object, IDictionary<string, object>> mToDictionaryDelegate;

        private readonly Func<IDictionary<string, object>, object> mToTupleDelegate;

        public ValueTupleDictionaryConverter(IList<string> transformNames, Type tupleType) :
            base(tupleType)
        {
            mTransformNames = transformNames;
            mTupleType = tupleType;

            Validate(transformNames, tupleType);
            mToDictionaryDelegate = BuildToDictionary();
            mToTupleDelegate = BuildToTuple();
        }

        private void Validate(IList<string> transformNames, Type tupleType)
        {
            ThrowHelper.ValidateSimpleTuple(tupleType);

            if (transformNames == null ||
                (transformNames.Count(x => x != null) != tupleType.GetValueTupleLength()))
            {
                throw new ArgumentException("Expected all tuple elements to have names");
            }
        }

        public object ToTuple(IDictionary<string, object> dictionary)
        {
            return mToTupleDelegate(dictionary);
        }

        public IDictionary<string, object> ToDictionary(object valueTuple)
        {
            return mToDictionaryDelegate(valueTuple);
        }

        private Func<IDictionary<string, object>, object> BuildToTuple()
        {
            ParameterExpression dictionary =
                Expression.Parameter(typeof(IDictionary<string, object>), "dictionary");

            IEnumerable<IndexExpression> itemsValues =
                mTransformNames.Select(name =>
                                       Expression.MakeIndex(dictionary, mDictionaryIndexer,
                                                            new Expression[] {Expression.Constant(name)}));

            // new ValueTuple<T1, T2, ..>((T1)dictionary["$argument1Name"],(T2)dictionary["$argument2Name"], ...)

            Func<IDictionary<string, object>, object> result =
                BuildToTuple<IDictionary<string, object>>
                    (dictionary, itemsValues);

            return result;
        }

        private Func<object, IDictionary<string, object>> BuildToDictionary()
        {
            ParameterExpression tupleResult =
                Expression.Parameter(typeof(object), "tupleResult");

            ParameterExpression casted =
                Expression.Variable(mTupleType, "casted");

            BinaryExpression assignment =
                Expression.Assign(casted, Expression.Convert(tupleResult, mTupleType));

            IEnumerable<KeyValuePair<string, Expression>> parameters =
                GetDictionaryInitializerExpressions(casted, mTupleType, mTransformNames);

            ListInitExpression dictionaryInit =
                Expression.ListInit
                    (Expression.New(typeof(Dictionary<string, object>)),
                     parameters.Select(x => Expression.ElementInit
                                           (mDictionaryAddMethod,
                                            Expression.Constant(x.Key, typeof(string)),
                                            Expression.Convert(x.Value, typeof(object)))));

            BlockExpression body =
                Expression.Block(variables: new[] {casted},
                                 expressions: new Expression[]
                                 {
                                     assignment,
                                     dictionaryInit
                                 });

            Expression<Func<object, IDictionary<string, object>>> lambda =
                Expression.Lambda<Func<object, IDictionary<string, object>>>
                    (body: body,
                     parameters: tupleResult);

            Func<object, IDictionary<string, object>> result = lambda.Compile();

            return result;
        }

        private static IEnumerable<KeyValuePair<string, Expression>> GetDictionaryInitializerExpressions
            (Expression tupleInstance,
             Type tupleType,
             IEnumerable<string> transformNames)
        {
            IEnumerable<Expression> tupleItemsExpressions = 
                GetTupleItemsExpressions(tupleInstance, tupleType);

            return tupleItemsExpressions.Zip
                (transformNames,
                 (expression, name) =>
                     new KeyValuePair<string, Expression>(name, expression));
        }
    }
}