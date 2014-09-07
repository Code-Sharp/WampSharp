using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Parsers;
using WampSharp.WebSocket4Net;
using WebSocket4Net;

namespace WampSharp.V1
{
    public static class WampChannelFactoryExtensions
    {
        public static IWampChannel<JToken> CreateChannel(this IWampChannelFactory<JToken> factory, string address)
        {
            return factory.CreateChannel(address, new JTokenMessageParser());
        }

        public static IWampChannel<TMessage> CreateChannel<TMessage>(this IWampChannelFactory<TMessage> factory, string address, IWampTextMessageParser<TMessage> parser)
        {
            return factory.CreateChannel(new WebSocket4NetTextConnection<TMessage>(address, 
                new Wamp1Binding<TMessage>(parser, factory.Formatter)));
        }

        public static IWampChannel<JToken> CreateChannel(this IWampChannelFactory<JToken> factory,
                                                         WebSocket socket)
        {
            return factory.CreateChannel(new WebSocket4NetTextConnection<JToken>(socket, new Wamp1Binding()));
        }
    }
}