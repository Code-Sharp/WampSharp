using Newtonsoft.Json;
using WampSharp.Binding;

namespace WampSharp.V2.Fluent
{
    public static class NewtonsoftMessagePackChannelFactoryExtensions
    {
        private static readonly JTokenMessagePackBinding mMessagePackBinding = new JTokenMessagePackBinding();

        /// <summary>
        /// Indicates that the user wants to use Msgpack serialization.
        /// </summary>
        public static ChannelFactorySyntax.ISerializationSyntax MessagePackSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = mMessagePackBinding;

            return state;
        }

        /// <summary>
        /// Indicates that the user wants to use Msgpack serialization.
        /// </summary>
        /// <param name="transportSyntax"></param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to serialize messages with.</param>
        public static ChannelFactorySyntax.ISerializationSyntax MessagePackSerialization(
            this ChannelFactorySyntax.ITransportSyntax transportSyntax, JsonSerializer serializer)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = new JTokenMessagePackBinding(serializer);

            return state;
        }
    }
}