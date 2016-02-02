using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    internal abstract class WampSerializedEvent : IWampSerializedEvent
    {
        private readonly long mPublicationId;
        private readonly EventDetails mDetails;
        private readonly ISerializedValue[] mArguments;
        private readonly IDictionary<string, ISerializedValue> mArgumentsKeywords;

        protected WampSerializedEvent(long publicationId, EventDetails details, ISerializedValue[] arguments, IDictionary<string, ISerializedValue> argumentsKeywords)
        {
            mPublicationId = publicationId;
            mDetails = details;
            mArguments = arguments;
            mArgumentsKeywords = argumentsKeywords;
        }

        public long PublicationId
        {
            get
            {
                return mPublicationId;
            }
        }

        public EventDetails Details
        {
            get
            {
                return mDetails;
            }
        }

        public ISerializedValue[] Arguments
        {
            get
            {
                return mArguments;
            }
        }

        public IDictionary<string, ISerializedValue> ArgumentsKeywords
        {
            get
            {
                return mArgumentsKeywords;
            }
        }
    }


    internal class WampSerializedEvent<TMessage> : WampSerializedEvent
    {
        public WampSerializedEvent(IWampFormatter<TMessage> formatter,
                                   long publicationId,
                                   EventDetails details,
                                   TMessage[] arguments = null) :
                                       this(formatter,
                                            publicationId,
                                            details,
                                            arguments,
                                            (IDictionary<string, TMessage>)null)
        {
        }

        public WampSerializedEvent(IWampFormatter<TMessage> formatter,
                                   long publicationId,
                                   EventDetails details,
                                   TMessage[] arguments,
                                   IDictionary<string, TMessage> argumentsKeywords) :
                                       this(formatter,
                                            publicationId,
                                            details,
                                            arguments,
                                            formatter.ConvertArgumentKeywords(argumentsKeywords))
        {
        }


        private WampSerializedEvent(IWampFormatter<TMessage> formatter,
                                    long publicationId,
                                    EventDetails details,
                                    TMessage[] arguments,
                                    IDictionary<string, ISerializedValue> argumentsKeywords) :
                                        base(publicationId,
                                             details,
                                             formatter.ConvertArguments(arguments),
                                             argumentsKeywords)
        {
        }
    }
}