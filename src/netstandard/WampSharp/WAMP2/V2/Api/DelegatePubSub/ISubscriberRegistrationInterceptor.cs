using System.Reflection;
using WampSharp.V2.Core.Contracts;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    /// <summary>
    /// Represents an interface that allows to get involved in subscriber registration.
    /// </summary>
    public interface ISubscriberRegistrationInterceptor
    {
        /// <summary>
        /// Returns a value indicating whether the given method is a
        /// topic event handler.
        /// </summary>
        /// <param name="method">The given method.</param>
        bool IsSubscriberHandler(MethodInfo method);

        /// <summary>
        /// Returns the topic uri of the given topic event handler.
        /// </summary>
        /// <param name="method">The given topic event handler.</param>
        string GetTopicUri(MethodInfo method);

        /// <summary>
        /// Returns the <see cref="SubscribeOptions"/> used to subscribe to the topic
        /// with the given topic event handler.
        /// </summary>
        /// <param name="method">The given topic event handler.</param>
        SubscribeOptions GetSubscribeOptions(MethodInfo method);
    }
}