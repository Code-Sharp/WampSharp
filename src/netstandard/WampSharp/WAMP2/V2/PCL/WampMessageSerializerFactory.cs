using WampSharp.Core.Proxy;
using WampSharp.V2;

namespace WampSharp.Core.Serialization
{
    /// <summary>
    /// An implementation of <see cref="IWampMessageSerializerFactory"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampMessageSerializerFactory<TMessage> : IWampMessageSerializerFactory
    {
        private readonly WampProtocol mProtocol;

        /// <summary>
        /// Creates a new instance of <see cref="WampMessageSerializerFactory{TMessage}"/>
        /// given a <see cref="IWampOutgoingRequestSerializer"/>, used to serialize
        /// message.
        /// </summary>
        /// <param name="serializer">The given <see cref="IWampOutgoingRequestSerializer"/>.</param>
        public WampMessageSerializerFactory(IWampOutgoingRequestSerializer serializer)
        {
            mProtocol = new WampProtocol(serializer);
        }

        public TProxy GetSerializer<TProxy>() where TProxy : class
        {
            TProxy result = mProtocol as TProxy;

            return result;
        }
    }
}