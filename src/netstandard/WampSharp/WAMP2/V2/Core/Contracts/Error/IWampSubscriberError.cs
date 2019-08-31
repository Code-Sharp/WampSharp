using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Handles errors of <see cref="IWampSubscriber"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampSubscriberError<TMessage>
    {
        /// <summary>
        /// Occurs when a SUBSCRIBE request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        [WampErrorHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error);

        /// <summary>
        /// Occurs when a SUBSCRIBE request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        [WampErrorHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments);

        /// <summary>
        /// Occurs when a SUBSCRIBE request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        /// <param name="argumentsKeywords">The error arguments keywords.</param>
        [WampErrorHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);

        /// <summary>
        /// Occurs when an UNSUBSCRIBE request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        [WampErrorHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error);

        /// <summary>
        /// Occurs when an UNSUBSCRIBE request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        [WampErrorHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments);

        /// <summary>
        /// Occurs when an UNSUBSCRIBE request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        /// <param name="argumentsKeywords">The error arguments keywords.</param>
        [WampErrorHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}