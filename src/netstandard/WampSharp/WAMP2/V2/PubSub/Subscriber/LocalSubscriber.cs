using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public abstract class LocalSubscriber : IWampRawTopicClientSubscriber
    {
        protected static readonly IWampFormatter<object> ObjectFormatter =
            WampObjectFormatter.Value;

        public abstract LocalParameter[] Parameters
        {
            get;
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details)
        {
            InnerEvent(formatter, publicationId, details, null, null);
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments)
        {
            InnerEvent(formatter, publicationId, details, arguments, null);
        }

        public void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments,
            IDictionary<string, TMessage> argumentsKeywords)
        {
            InnerEvent(formatter, publicationId, details, arguments, argumentsKeywords);
        }

        protected object[] UnpackParameters<TMessage>
            (IWampFormatter<TMessage> formatter,
                TMessage[] arguments,
                IDictionary<string, TMessage> argumentsKeywords)
        {
            ArgumentUnpacker unpacker = new ArgumentUnpacker(Parameters);

            object[] result =
                unpacker.UnpackParameters(formatter, arguments, argumentsKeywords)
                        .ToArray();

            return result;
        }

        protected abstract void InnerEvent<TMessage>
            (IWampFormatter<TMessage> formatter,
                long publicationId,
                EventDetails details,
                TMessage[] arguments,
                IDictionary<string, TMessage> argumentsKeywords);
    }
}