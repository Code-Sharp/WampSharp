using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Core.Serialization
{
    /// <summary>
    /// An implementation of <see cref="IWampMessageSerializerFactory"/>.
    /// </summary>
    public class WampMessageSerializerFactory : IWampMessageSerializerFactory
    {
        private readonly WampProtocol mProtocol;

        /// <summary>
        /// Creates a new instance of <see cref="WampMessageSerializerFactory"/>
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