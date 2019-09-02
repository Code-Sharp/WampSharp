using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core;

namespace WampSharp.V2
{
    /// <summary>
    /// A default implementation of <see cref="IWampEventValueTupleConverter{TTuple}"/>.
    /// </summary>
    /// <typeparam name="TTuple"></typeparam>
    /// <remarks>
    /// Derive from this class and specify your desired tuple type.
    /// </remarks>
    /// <example>
    /// public class MyEventValueTupleConverter : WampEventValueTupleConverter{(int x, int y)}
    /// {
    /// }
    /// </example>
    public abstract class WampEventValueTupleConverter<TTuple> : IWampEventValueTupleConverter<TTuple>
    {
        private readonly ArgumentUnpacker mArgumentUnpacker;
        private readonly ValueTupleArrayConverter mArrayConverter;
        private readonly ValueTupleDictionaryConverter mDictionaryConverter;
        private static readonly object[] mEmptyArguments = new object[0];

        protected WampEventValueTupleConverter()
        {
            Type tupleType = typeof(TTuple);

            if (!tupleType.IsValueTuple())
            {
                throw new ArgumentException("Expected TTuple to be a ValueTuple");
            }

            if (!tupleType.IsValidTupleType())
            {
                throw new ArgumentException("TTuple is an invalid ValueTuple. Expected TRest to be a ValueTuple.");
            }

            mArrayConverter = ValueTupleArrayConverter<TTuple>.Value;

            Type converterType = GetConverterType();

            ValidateConverterType(converterType);

            IList<string> transformNames = GetTransformNames(converterType);

            int tupleLength = tupleType.GetValueTupleLength();

            ValidateTransformNames(transformNames, tupleLength);

            if (transformNames != null)
            {
                if (transformNames.Take(tupleLength).All(x => x != null))
                {
                    mDictionaryConverter =
                        ValueTupleDictionaryConverterBuilder.Build(tupleType, transformNames);
                }
            }

            mArgumentUnpacker =
                ArgumentUnpackerHelper.GetValueTupleArgumentUnpacker
                    (tupleType, transformNames);
        }

        private void ValidateConverterType(Type converterType)
        {
            if (converterType.IsGenericType)
            {
                Type genericTypeDefinition = 
                    converterType.GetGenericTypeDefinition();

                Type genericTypeDefinitionBase =
                    genericTypeDefinition.BaseType;

                Type genericTypeDefinitionBaseTupleType =
                    genericTypeDefinitionBase.GetGenericArguments()[0];

                if (!genericTypeDefinitionBaseTupleType.IsValueTuple())
                {
                    throw new ArgumentException(
                                                $"Expected a class deriving directly from {typeof(WampEventValueTupleConverter<>).Name} to specify a ValueTuple as the generic parameter TTuple");
                }
            }
        }

        private void ValidateTransformNames(IList<string> transformNames, int tupleLength)
        {
            if (transformNames != null)
            {
                IEnumerable<string> relevantNames =
                    transformNames.Take(tupleLength).ToList();

                if (!(relevantNames.All(x => x == null) ||
                      relevantNames.All(x => x != null)))
                {
                    throw new ArgumentException(
                        "Expected all TTuple elements to have a name or non of them to have a name");
                }
            }
        }

        private IList<string> GetTransformNames(Type converterType)
        {
            IList<string> transformNames = null;

            if (converterType.IsDefined(typeof(TupleElementNamesAttribute), true))
            {
                TupleElementNamesAttribute attribute =
                    converterType.GetCustomAttribute<TupleElementNamesAttribute>();

                transformNames = attribute.TransformNames;
            }

            return transformNames;
        }

        private Type GetConverterType()
        {
            Type currentType = this.GetType();

            Type baseType = typeof(WampEventValueTupleConverter<TTuple>);

            while (currentType.BaseType != baseType)
            {
                currentType = currentType.BaseType;
            }

            return currentType;
        }

        public virtual TTuple ToTuple(IWampSerializedEvent @event)
        {
            return ToTuple(SerializedValueFormatter.Value, @event.Arguments, @event.ArgumentsKeywords);
        }

        public virtual TTuple ToTuple<TMessage>(IWampFormatter<TMessage> formatter,
                                                TMessage[] argumentsArray,
                                                IDictionary<string, TMessage> argumentKeywords)
        {
            object[] unpacked =
                mArgumentUnpacker.UnpackParameters(formatter,
                                                   argumentsArray,
                                                   argumentKeywords)
                                 .ToArray();

            return (TTuple) mArrayConverter.ToTuple(unpacked);
        }

        public virtual IWampEvent ToEvent(TTuple tuple)
        {
            IDictionary<string, object> argumentsKeywords = null;

            object[] arguments = mEmptyArguments;

            if (mDictionaryConverter != null)
            {
                argumentsKeywords = mDictionaryConverter.ToDictionary(tuple);
            }
            else
            {
                arguments = mArrayConverter.ToArray(tuple);
            }

            WampEvent result =
                new WampEvent
                {
                    Arguments = arguments,
                    ArgumentsKeywords = argumentsKeywords
                };

            return result;
        }
    }
}