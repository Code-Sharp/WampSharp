using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal static class ErrorExtractor
    {
        private static IDictionary<string, object> DeserializeDictionary<TMessage>(TMessage details, IWampFormatter<TMessage> formatter)
        {
            return formatter.Deserialize<IDictionary<string, object>>(details);
        }

        public static WampException Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error)
        {
            IDictionary<string, object> deserializedDetails =
                DeserializeDictionary(details, formatter);

            WampException exception = new WampException(deserializedDetails, error);
            
            return exception;
        }

        public static WampException Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments)
        {
            IDictionary<string, object> deserializedDetails =
                DeserializeDictionary(details, formatter);

            object[] castedArguments = arguments.Cast<object>().ToArray();

            WampException exception = new WampException(deserializedDetails, error, castedArguments);
            return exception;
        }

        public static WampException Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords)
        {
            IDictionary<string, object> deserializedDetails =
                DeserializeDictionary(details, formatter);

            object[] castedArguments = arguments.Cast<object>().ToArray();

            IDictionary<string, object> deserializedArgumentKeywords =
                DeserializeDictionary(argumentsKeywords, formatter);

            WampException exception = new WampException(deserializedDetails, error, castedArguments,
                                                        deserializedArgumentKeywords);
            return exception;
        }
    }
}