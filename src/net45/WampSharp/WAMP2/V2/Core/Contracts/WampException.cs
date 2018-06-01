using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampException : Exception
    {
        private readonly object[] mArguments;
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
            mArguments = arguments;
            ArgumentsKeywords = argumentsKeywords;
        }

        public WampException(IDictionary<string, object> details, string errorUri, string message,
                             IDictionary<string, object> argumentsKeywords)
            : base(message)
        {
            ErrorUri = errorUri;
            Details = details ?? mEmptyDetails;
            mArguments = new object[] {message};
            ArgumentsKeywords = argumentsKeywords;
        }

        public WampException(string errorUri, string messageDetails) :
            base($"Error uri: '{errorUri}', details: {messageDetails}")
        {
            ErrorUri = errorUri;
            mArguments = new object[] {messageDetails};
        }

        public WampException(IDictionary<string, object> details, string errorUri, object[] arguments,
                             IDictionary<string, object> argumentsKeywords, string message, Exception inner)
            : base(message, inner)
        {
            ErrorUri = errorUri;
            Details = details ?? mEmptyDetails;
            mArguments = arguments;
            ArgumentsKeywords = argumentsKeywords;
        }

#if !PCL
        protected WampException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        public string ErrorUri { get; }

        public IDictionary<string, object> Details { get; }

        public object[] Arguments => mArguments;

        public IDictionary<string, object> ArgumentsKeywords { get; }
    }
}