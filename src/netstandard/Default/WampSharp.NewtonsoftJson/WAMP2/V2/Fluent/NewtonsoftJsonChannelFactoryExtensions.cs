using Newtonsoft.Json;
using WampSharp.Binding;

namespace WampSharp.V2.Fluent
{
    public static class NewtonsoftJsonChannelFactoryExtensions
    {
        private static readonly JTokenJsonBinding mJsonBinding = new JTokenJsonBinding();

        /// <summary>
        /// Indicates that the user wants to use JSON serialization.
        /// </summary>
        public static ChannelFactorySyntax.ISerializationSyntax JsonSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = mJsonBinding;

            return state;
        }

        /// <summary>
        /// Indicates that the user wants to use JSON serialization.
        /// </summary>
        /// <param name="transportSyntax">The current fluent syntax state.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to serialize messages with.</param>
        public static ChannelFactorySyntax.ISerializationSyntax JsonSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax, JsonSerializer serializer)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = new JTokenJsonBinding(serializer);

            return state;
        }
    }
}