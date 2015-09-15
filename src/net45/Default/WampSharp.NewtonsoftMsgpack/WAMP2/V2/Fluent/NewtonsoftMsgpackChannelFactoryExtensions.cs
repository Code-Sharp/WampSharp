using Newtonsoft.Json;
using WampSharp.Binding;

namespace WampSharp.V2.Fluent
{
    public static class NewtonsoftMsgpackChannelFactoryExtensions
    {
        private static readonly JTokenMsgpackBinding mMsgpackBinding = new JTokenMsgpackBinding();

        public static ChannelFactorySyntax.ISerializationSyntax MsgpackSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = mMsgpackBinding;

            return state;
        }

        public static ChannelFactorySyntax.ISerializationSyntax MsgpackSerialization(this ChannelFactorySyntax.ITransportSyntax transportSyntax, JsonSerializer serializer)
        {
            ChannelState state = transportSyntax.State;

            state.Binding = new JTokenMsgpackBinding(serializer);

            return state;
        }
    }
}