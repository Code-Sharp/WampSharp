using System.Reflection;
using WampSharp.Core.Message;

namespace WampSharp.Core.Proxy
{
    /// <summary>
    /// Serializes method calls into <see cref="WampMessage{TMessage}"/>s.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampOutgoingRequestSerializer<TMessage>
    {
        /// <summary>
        /// Serializes a method call into a <see cref="WampMessage{TMessage}"/>.
        /// </summary>
        /// <param name="method">The method that was called.</param>
        /// <param name="arguments">The arguments of the call.</param>
        /// <returns>The serialized <see cref="WampMessage{TMessage}"/>.</returns>
        WampMessage<TMessage> SerializeRequest(MethodInfo method, object[] arguments);
    }
}