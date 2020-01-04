using Newtonsoft.Json;
using WampSharp.Binding;

namespace WampSharp.V2.Fluent
{
    public static class NewtonsoftMsgpackChannelFactoryExtensions
    {
        private static readonly JTokenMsgpackBinding mMsgpackBinding = new JTokenMsgpackBinding();

        /// <summary>
        /// Indicates that the user wants to use Msgpack serialization.
        /// </summary>
        public static ChannelFactorySyntax.ISerializationSyntax MsgpackSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = mMsgpackBinding;

            return state;
        }

        /// <summary>
        /// Indicates that the user wants to use Msgpack serialization.
        /// </summary>
        /// <param name="transportSyntax">The current fluent syntax state.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> to serialize messages with.</param>
        public static ChannelFactorySyntax.ISerializationSyntax MsgpackSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax, JsonSerializer serializer)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = new JTokenMsgpackBinding(serializer);

            return state;
        }
    }
}