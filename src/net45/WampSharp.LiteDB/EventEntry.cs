using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LiteDB;

namespace WampSharp.LiteDB
{
    internal class EventEntry
    {
        [BsonRef("subscriptions")]
        public SubscriptionEntry Subscription { get; set; }

        public int Id { get; set; }

        [DataMember(Name = "timestamp")]
        public DateTime Timestamp { get; set; }

        [DataMember(Name = "publisher")]
        public long PublisherId { get; set; }

        [DataMember(Name = "publication")]
        public long PublicationId { get; set; }

        [DataMember(Name = "topic")]
        public string Topic { get; internal set; }

        [DataMember(Name = "args")]
        public BsonValue[] Arguments { get; set; }

        [DataMember(Name = "kwargs")]
        public IDictionary<string, BsonValue> ArgumentsKeywords { get; set; }
    }
}