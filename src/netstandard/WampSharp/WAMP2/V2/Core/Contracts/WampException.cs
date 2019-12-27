using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampException : Exception
    {
        private static readonly IDictionary<string, object> mEmptyDetails =
            new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());

        public WampException(string errorUri, params object[] arguments) :
            this(errorUri, arguments, null)
        {
        }

        public WampException(string errorUri, object[] arguments, IDictionary<string, object> argumentsKeywords) :
            this(mEmptyDetails, errorUri, arguments, argumentsKeywords)
        {
        }

        public WampException(IDictionary<string, object> details, string errorUri) : this(details, errorUri, (object[])null, null)
        {
        }

        public WampException(IDictionary<string, object> details, string errorUri, object[] arguments) : this(details, errorUri, arguments, null)
        {
        }

        public WampException(IDictionary<string, object> details, string errorUri, object[] arguments,
                             IDictionary<string, object> argumentsKeywords) : base(errorUri)
        {
            ErrorUri = errorUri;
            Details = details ?? mEmptyDetails;
            Arguments = arguments;
            ArgumentsKeywords = argumentsKeywords;
        }

        public WampException(IDictionary<string, object> details, string errorUri, string message,
                             IDictionary<string, object> argumentsKeywords)
            : base(message)
        {
            ErrorUri = errorUri;
            Details = details ?? mEmptyDetails;
            Arguments = new object[] {message};
            ArgumentsKeywords = argumentsKeywords;
        }

        public WampException(string errorUri, string messageDetails) :
            base($"Error uri: '{errorUri}', details: {messageDetails}")
        {
            ErrorUri = errorUri;
            Details = mEmptyDetails;
            Arguments = new object[] {messageDetails};
        }

        public WampException(IDictionary<string, object> details, string errorUri, object[] arguments,
                             IDictionary<string, object> argumentsKeywords, string message, Exception inner)
            : base(message, inner)
        {
            ErrorUri = errorUri;
            Details = details ?? mEmptyDetails;
            Arguments = arguments;
            ArgumentsKeywords = argumentsKeywords;
        }

        protected WampException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string ErrorUri { get; }

        public IDictionary<string, object> Details { get; }

        public object[] Arguments { get; private set; }

        public IDictionary<string, object> ArgumentsKeywords { get; }
    }
}