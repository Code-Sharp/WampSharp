using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Handles errors of <see cref="IWampCallee"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampCalleeError<TMessage>
    {
        /// <summary>
        /// Occurs when a REGISTER request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        [WampErrorHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error);

        /// <summary>
        /// Occurs when a REGISTER request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        [WampErrorHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments);

        /// <summary>
        /// Occurs when a REGISTER request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        /// <param name="argumentsKeywords">The error arguments keywords.</param>
        [WampErrorHandler(WampMessageType.v2Register)]
        void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);

        /// <summary>
        /// Occurs when an UNREGISTER request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        [WampErrorHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error);

        /// <summary>
        /// Occurs when an UNREGISTER request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        [WampErrorHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments);

        /// <summary>
        /// Occurs when an UNREGISTER request error arrives.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <param name="details">The request details.</param>
        /// <param name="error">The error uri.</param>
        /// <param name="arguments">The error arguments.</param>
        /// <param name="argumentsKeywords">The error arguments keywords.</param>
        [WampErrorHandler(WampMessageType.v2Unregister)]
        void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}