using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch.Handler
{
    /// <summary>
    /// Maps WAMP requests to their corresponding method.
    /// </summary>
    public interface IWampRequestMapper<TMessage>
    {
        /// <summary>
        /// Maps the given WAMP request to its corresponding method.
        /// </summary>
        /// <param name="request">The given WAMP request.</param>
        /// <returns>The given request's corresponding method.</returns>
        WampMethodInfo Map(WampMessage<TMessage> request);
    }
}