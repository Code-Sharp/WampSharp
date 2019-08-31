namespace WampSharp.V2.Client
{
    /// <summary>
    /// Represents a proxy to a WAMP topic
    /// </summary>
    public interface IWampTopicContainerProxy
    {
        /// <summary>
        /// Gets a <see cref="IWampTopicProxy"/> given a WAMP topic uri.
        /// </summary>
        /// <param name="topicUri">The given topic uri.</param>
        /// <returns>The requested <see cref="IWampTopicProxy"/>.</returns>
        IWampTopicProxy GetTopicByUri(string topicUri);
    }
}