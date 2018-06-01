using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WampSharp.Core.Utilities.ValueTuple
{
    internal static class ValueTupleDictionaryConverterBuilder
    {
        private static readonly MethodInfo mBuildMethod = Method.Get(() => Build<object>(default(IList<string>)))
                                                                .GetGenericMethodDefinition();

        public static ValueTupleDictionaryConverter Build(Type tupleType, IList<string> transformNames)
        {
            MethodInfo methodToInvoke = 
                mBuildMethod.MakeGenericMethod(tupleType);

            object result =
                methodToInvoke.Invoke(null, new object[] {transformNames});

            return (ValueTupleDictionaryConverter) result;
        }

        public static ValueTupleDictionaryConverter Build<TTuple>(IList<string> transformNames)
        {
            return ValueTupleDictionaryConverterBuilder<TTuple>.Get(transformNames);
        }
    }

    internal class ValueTupleDictionaryConverterBuilder<TTuple>
    {
        private static readonly MethodInfo mDictionaryAddMethod =
            Method.Get((IDictionary<string, object> dictionary) =>
                           dictionary.Add(default(string), default(object)));

        private static readonly PropertyInfo mDictionaryIndexer =
            typeof(IDictionary<string, object>).GetProperties().
                                                FirstOrDefault(x => x.GetIndexParameters().Any());

        private static readonly PropertyInfo mListIndexer =
            typeof(IList<string>).GetProperties().
                                 FirstOrDefault(x => x.GetIndexParameters().Any());

        private static readonly Func<object, IList<string>, IDictionary<string, object>> mToDictionary = GetToDictionaryBuilder();
        private static readonly Func<IDictionary<string, object>, IList<string>, object> mToTuple = GetToTupleBuilder();

        public static ValueTupleDictionaryConverter Get(IList<string> transformNames)
        {
            Func<object, IDictionary<string, object>> toDictionaryDelegate =
                tuple => mToDictionary(tuple, transformNames);

            Func<IDictionary<string, object>, object> toTupleDelegate = 
                dictionary => mToTuple(dictionary, transformNames); 

            return new ValueTupleDictionaryConverter(typeof(TTuple), toDictionaryDelegate, toTupleDelegate);
        }

        private static Func<object, IList<string>, IDictionary<string, object>> GetToDictionaryBuilder()
        {
            Type tupleType = typeof(TTuple);

            ParameterExpression transformNamesParameter =
                Expression.Parameter(typeof(IList<string>), "transformNamesParameter");

            ParameterExpression tupleParameter =
                Expression.Parameter(typeof(object), "tupleParameter");

            ParameterExpression casted =
                Expression.Variable(tupleType, "casted");

            BinaryExpression assignment =
                Expression.Assign(casted, Expression.Convert(tupleParameter, tupleType));

            IEnumerable<Expression> tupleItemsExpressions =
                ValueTupleConverter.GetTupleItemsExpressions(casted, tupleType);

            ListInitExpression dictionaryInit =
                Expression.ListInit
                (Expression.New(typeof(Dictionary<string, object>)),
                 tupleItemsExpressions.Select((parameter, i) => Expression.ElementInit
                                   (mDictionaryAddMethod,
                                    Expression.Property(transformNamesParameter, mListIndexer, Expression.Constant(i)),
                                    parameter)));

            BlockExpression body =
                Expression.Block(variables: new[] { casted },
                                 expressions: new Expression[]
                                 {
                                     assignment,
                                     dictionaryInit
                                 });

            Expression<Func<object, IList<string>, IDictionary<string, object>>> lambda =
                Expression.Lambda<Func<object, IList<string>, IDictionary<string, object>>>
                    (body,
                     tupleParameter,
                     transformNamesParameter);

            Func<object, IList<string>, IDictionary<string, object>> result = lambda.Compile();

            return result;
        }

        private static Func<IDictionary<string, object>, IList<string>, object> GetToTupleBuilder()
        {
            ParameterExpression dictionary =
                Expression.Parameter(typeof(IDictionary<string, object>), "dictionary");

            ParameterExpression transformNames =
                Expression.Parameter(typeof(IList<string>), "transformNames");

            int valueTupleLength = typeof(TTuple).GetValueTupleLength();

            var tupleElements =
                Enumerable.Range(0, valueTupleLength);

            IEnumerable<IndexExpression> itemsValues =
                tupleElements.Select
                (index =>
                     Expression.MakeIndex(dictionary, mDictionaryIndexer,
                                          new Expression[]
                                          {
                                              Expression.Property(transformNames,
                                                                  mListIndexer,
                                                                  Expression.Constant(index))
                                          }));

            // new ValueTuple<T1, T2, ..>((T1)dictionary[transformNames[0]],(T2)dictionary[transformNames[1]], ...)
            Func<IDictionary<string, object>, IList<string>, object> result =
                GetToTupleBuilder(dictionary, itemsValues, transformNames);

            return result;
        }

        private static Func<IDictionary<string, object>, IList<string>, object> GetToTupleBuilder(ParameterExpression dictionary, IEnumerable<IndexExpression> itemsValues, ParameterExpression transformNames)
        {
            // new ValueTuple<T1, T2, ..>((T1)dictionary[transformNames[0]],(T2)dictionary[transformNames[1]], ...)
            NewExpression tupleCreation =
                ValueTupleConverter.GetTupleNewExpression(itemsValues, typeof(TTuple));

            UnaryExpression boxed =
                Expression.Convert(tupleCreation, typeof(object));

            var lambda =
                Expression.Lambda<Func<IDictionary<string, object>, IList<string>, object>>
                (boxed, dictionary, transformNames);

            var result = lambda.Compile();

            return result;
        }
    }
}