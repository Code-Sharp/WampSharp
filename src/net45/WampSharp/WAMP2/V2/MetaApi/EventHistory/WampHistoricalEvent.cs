using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    public class WampHistoricalEvent : WampHistoricalEvent<ISerializedValue>
    {
    }

    public class WampHistoricalEvent<TMessage>
    {
        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; internal set; }

        [DataMember(Name = "publisher")]
        public long PublisherId { get; internal set; }

        [DataMember(Name = "publication")]
        public long PublicationId { get; internal set; }

        [DataMember(Name = "topic")]
        public string Topic { get; internal set; }

        [DataMember(Name = "args")]
        public TMessage[] Arguments { get; internal set; }

        [DataMember(Name = "kwargs")]
        public IDictionary<string, TMessage> ArgumentsKeywords { get; internal set; }
    }
}