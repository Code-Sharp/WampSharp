using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2
{
    internal static class SerializedValueExtensions
    {
        private static ISerializedValue GetSerializedArgument<TMessage>(IWampFormatter<TMessage> formatter, TMessage argument)
        {
            if (argument == null)
            {
                return null;
            }
            else
            {
                return new SerializedValue<TMessage>(formatter, argument);
            }
        }

        public static ISerializedValue[] ConvertArguments<TMessage>(this IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            if (arguments == null)
            {
                return null;
            }
            else
            {
                return arguments.Select(x => new SerializedValue<TMessage>(formatter, x)).Cast<ISerializedValue>().ToArray();
            }
        }

        public static IDictionary<string, ISerializedValue> ConvertArgumentKeywords<TMessage>(this IWampFormatter<TMessage> formatter, IDictionary<string, TMessage> argument)
        {
            if (argument == null)
            {
                return null;
            }
            else
            {
                Dictionary<string, ISerializedValue> result =
                    argument.ToDictionary(x => x.Key,
                                          x => GetSerializedArgument<TMessage>(formatter, x.Value));

                return result;
            }
        }
    }
}