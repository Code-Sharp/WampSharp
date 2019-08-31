using Newtonsoft.Json;
using WampSharp.Binding;

namespace WampSharp.V2.Fluent
{
    public static class NewtonsoftCborChannelFactoryExtensions
    {
        private static readonly JTokenCborBinding mCborBinding = new JTokenCborBinding();

        /// <summary>
        /// Indicates that the user wants to use cbor serialization.
        /// </summary>
        public static ChannelFactorySyntax.ISerializationSyntax CborSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = mCborBinding;

            return state;
        }

        /// <summary>
        /// Indicates that the user wants to use cbor serialization.
        /// </summary>
        /// <param name="transportSyntax"></param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to serialize messages with.</param>
        public static ChannelFactorySyntax.ISerializationSyntax CborSerialization(
            this ChannelFactorySyntax.ITransportSyntax transportSyntax, JsonSerializer serializer)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = new JTokenCborBinding(serializer);

            return state;
        }
    }
}