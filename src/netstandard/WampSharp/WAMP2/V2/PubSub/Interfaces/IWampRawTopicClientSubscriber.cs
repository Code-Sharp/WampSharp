using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    /// <summary>
    /// Represents a topic subscriber that lives outside a router process.
    /// </summary>
    public interface IWampRawTopicClientSubscriber
    {
        /// <summary>
        /// Occurs when an incoming event is avilable.
        /// </summary>
        /// <param name="formatter">A formatter that can be used to deserialize event arguments.</param>
        /// <param name="publicationId">The publication id of the incoming publication.</param>
        /// <param name="details">The details about this publication.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details);

        /// <summary>
        /// Occurs when an incoming event is avilable.
        /// </summary>
        /// <param name="formatter">A formatter that can be used to deserialize event arguments.</param>
        /// <param name="publicationId">The publication id of the incoming publication.</param>
        /// <param name="details">The details about this publication.</param>
        /// <param name="arguments">The arguments of this publication.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments);

        /// <summary>
        /// Occurs when an incoming event is avilable.
        /// </summary>
        /// <param name="formatter">A formatter that can be used to deserialize event arguments.</param>
        /// <param name="publicationId">The publication id of the incoming publication.</param>
        /// <param name="details">The details about this publication.</param>
        /// <param name="arguments">The arguments of this publication.</param>
        /// <param name="argumentsKeywords">The arguments keywords of this publication.</param>
        /// <typeparam name="TMessage"></typeparam>
        void Event<TMessage>(IWampFormatter<TMessage> formatter, long publicationId, EventDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}