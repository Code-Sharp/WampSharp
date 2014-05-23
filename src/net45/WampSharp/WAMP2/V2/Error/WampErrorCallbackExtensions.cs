using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Error
{
    internal static class WampErrorCallbackExtensions
    {
        private static readonly object[] mEmptyArguments = new object[0];

        public static void Error
            (this IWampErrorCallback client,
             WampException exception)
        {
            IDictionary<string, object> details = exception.Details;

            string errorUri = exception.ErrorUri;

            IDictionary<string, object> argumentsKeywords = exception.ArgumentsKeywords;

            object[] arguments = exception.Arguments;

            if (argumentsKeywords != null)
            {
                if (arguments == null)
                {
                    arguments = mEmptyArguments;
                }

                client.Error(details, errorUri, arguments, argumentsKeywords);
            }
            else if (arguments != null)
            {
                client.Error(details, errorUri, arguments);
            }
            else
            {
                client.Error(details, errorUri);
            }
        }
    }
}