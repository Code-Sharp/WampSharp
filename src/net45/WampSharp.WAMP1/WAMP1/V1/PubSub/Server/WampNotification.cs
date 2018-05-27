using System.Collections.Generic;

namespace WampSharp.V1.PubSub.Server
{
    /// <summary>
    /// Represents a <see cref="IWampTopic"/>'s publication.
    /// </summary>
    public class WampNotification
    {
        private readonly ICollection<string> mEligible;
        private static readonly string[] mEmptyStringArray = new string[0];

        /// <summary>
        /// Initializes a new instance of <see cref="WampNotification"/>.
        /// </summary>
        /// <param name="event">The event to publish</param>
        public WampNotification(object @event) :
            this(@event, mEmptyStringArray, mEmptyStringArray)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="WampNotification"/>.
        /// </summary>
        /// <param name="event">The event to publish</param>
        /// <param name="excluded">A collection of excluded observers' session ids.</param>
        /// <param name="eligible">A collection of eligible observers' session ids.</param>
        public WampNotification(object @event, ICollection<string> excluded, ICollection<string> eligible)
        {
            Event = @event;
            Excluded = excluded;
            mEligible = eligible;
        }

        /// <summary>
        /// Gets the published event.
        /// </summary>
        public object Event { get; }

        /// <summary>
        /// Gets a collection of excluded observers' session ids.
        /// </summary>
        public ICollection<string> Excluded { get; }

        /// <summary>
        /// Gets a collection of eligible observers' session ids.
        /// </summary>
        public ICollection<string> Eligible => mEligible;
    }
}