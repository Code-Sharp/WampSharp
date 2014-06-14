using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2
{
    internal abstract class WampSerializedEvent : IWampSerializedEvent
    {
        private readonly long mPublicationId;
        private readonly ISerializedValue mDetails;
        private readonly ISerializedValue[] mArguments;
        private readonly ISerializedValue mArgumentsKeywords;

        protected WampSerializedEvent(long publicationId, ISerializedValue details, ISerializedValue[] arguments, ISerializedValue argumentsKeywords)
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

        public ISerializedValue Details
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

        public ISerializedValue ArgumentsKeywords
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
                                   TMessage details,
                                   TMessage[] arguments = null) :
                                       this(formatter,
                                            publicationId,
                                            details,
                                            arguments,
                                            null)
        {
        }

        public WampSerializedEvent(IWampFormatter<TMessage> formatter,
                                   long publicationId,
                                   TMessage details,
                                   TMessage[] arguments,
                                   TMessage argumentsKeywords) :
                                       this(formatter,
                                            publicationId,
                                            details,
                                            arguments,
                                            GetSerializedArgument(formatter, argumentsKeywords))
        {
        }


        private WampSerializedEvent(IWampFormatter<TMessage> formatter,
                                    long publicationId,
                                    TMessage details,
                                    TMessage[] arguments,
                                    ISerializedValue argumentsKeywords) :
                                        base(publicationId,
                                             GetSerializedArgument(formatter, details),
                                             GetSerializedArguments(formatter, arguments),
                                             argumentsKeywords)
        {
        }

        private static SerializedValue<TMessage> GetSerializedArgument(IWampFormatter<TMessage> formatter, TMessage argument)
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

        private static ISerializedValue[] GetSerializedArguments(IWampFormatter<TMessage> formatter, TMessage[] arguments)
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
    }
}