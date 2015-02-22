using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.PubSub
{
    public abstract class LocalSubscriber : IWampRawTopicClientSubscriber
    {
        private readonly string mTopic;

        protected readonly static IWampFormatter<object> ObjectFormatter =
            WampObjectFormatter.Value;

        protected LocalSubscriber(string topic)
        {
            mTopic = topic;
        }

        public string Topic
        {
            get
            {
                return mTopic;
            }
        }

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
                unpacker.UnpackParameters(formatter, arguments, argumentsKeywords);

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