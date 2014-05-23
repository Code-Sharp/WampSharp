using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace WampSharp.V2.Core.Contracts
{
    [Serializable]
    public class WampException : Exception
    {
        private readonly string mErrorUri;
        private readonly IDictionary<string, object> mDetails;
        private readonly object[] mArguments;
        private readonly IDictionary<string, object> mArgumentsKeywords;

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
                             IDictionary<string, object> argumentsKeywords)
        {
            mErrorUri = errorUri;
            mDetails = details;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public WampException(IDictionary<string, object> details, string errorUri, string message,
                             IDictionary<string, object> argumentsKeywords)
            : base(message)
        {
            mErrorUri = errorUri;
            mDetails = details;
            mArguments = new object[] {message};
            mArgumentsKeywords = argumentsKeywords;
        }

        public WampException(IDictionary<string, object> details, string errorUri, object[] arguments,
                             IDictionary<string, object> argumentsKeywords, string message, Exception inner)
            : base(message, inner)
        {
            mErrorUri = errorUri;
            mDetails = details;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        protected WampException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public string ErrorUri
        {
            get
            {
                return mErrorUri;
            }
        }

        public IDictionary<string, object> Details
        {
            get
            {
                return mDetails;
            }
        }

        public object[] Arguments
        {
            get
            {
                return mArguments;
            }
        }

        public IDictionary<string, object> ArgumentsKeywords
        {
            get
            {
                return mArgumentsKeywords;
            }
        }
    }
}