using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a given WAMP session authorizer - i.e. a mechanism
    /// that determines whether a client is allowed to perform requested
    /// action.
    /// </summary>
    public interface IWampAuthorizer
    {
        /// <summary>
        /// Returns a value indicating whether this client is allowed to perform the 
        /// requested registration.
        /// </summary>
        /// <param name="options">The requested registration options.</param>
        /// <param name="procedure">The requested procedure to register.</param>
        /// <returns>A value indicating whether the requested registration is allowed 
        /// for this client.</returns>
        bool CanRegister(RegisterOptions options, string procedure);

        /// <summary>
        /// Returns a value indicating whether this client is allowed to perform the 
        /// requested call.
        /// </summary>
        /// <param name="options">The requested call options.</param>
        /// <param name="procedure">The requested procedure to call.</param>
        /// <returns>A value indicating whether the requested call is allowed 
        /// for this client.</returns>
        bool CanCall(CallOptions options, string procedure);

        /// <summary>
        /// Returns a value indicating whether this client is allowed to perform the 
        /// requested publication.
        /// </summary>
        /// <param name="options">The requested publish options.</param>
        /// <param name="topicUri">The requested topic to publish to.</param>
        /// <returns>A value indicating whether the requested publication is allowed 
        /// for this client.</returns>
        bool CanPublish(PublishOptions options, string topicUri);

        /// <summary>
        /// Returns a value indicating whether this client is allowed to perform the 
        /// requested subscription.
        /// </summary>
        /// <param name="options">The requested subscribe options.</param>
        /// <param name="topicUri">The requested topic to subscribe to.</param>
        /// <returns>A value indicating whether the requested subscription is allowed 
        /// for this client.</returns>
        bool CanSubscribe(SubscribeOptions options, string topicUri);
    }
}