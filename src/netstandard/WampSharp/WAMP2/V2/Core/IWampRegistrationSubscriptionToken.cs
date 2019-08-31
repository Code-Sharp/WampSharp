using System;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Core
{
    /// <summary>
    /// Represents a result from <see cref="IWampRpcOperationCatalog.Register"/> or
    /// <see cref="IWampTopicContainer.Subscribe"/> - includes the registration/subscription id.
    /// Disposing it removes the subscription/registration.
    /// </summary>
    public interface IWampRegistrationSubscriptionToken : IDisposable
    {
        /// <summary>
        /// Gets the subscription/registration id of the result of the
        /// SUBSCRIBED/REGISTRATION message.
        /// </summary>
        long TokenId { get; }
    }
}