using Newtonsoft.Json;
using WampSharp.Binding;

namespace WampSharp.V2.Fluent
{
    public static class NewtonsoftJsonChannelFactoryExtensions
    {
        private static readonly JTokenJsonBinding mJsonBinding = new JTokenJsonBinding();

        public static ChannelFactorySyntax.ISerializationSyntax JsonSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = mJsonBinding;

            return state;
        }

        public static ChannelFactorySyntax.ISerializationSyntax JsonSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax, JsonSerializer serializer)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = new JTokenJsonBinding(serializer);

            return state;
        }
    }
}