using System;
using Newtonsoft.Json.Linq;
using SuperSocket.ClientEngine;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Parsers;
using WampSharp.WebSocket4Net;
using WebSocket4Net;

namespace WampSharp.V1
{
    public static class WampChannelFactoryExtensions
    {
        public static IWampChannel<JToken> CreateChannel(this DefaultWampChannelFactory factory, string address)
        {
            return factory.CreateChannel(address, null);
        }

        public static IWampChannel<JToken> CreateChannel(this DefaultWampChannelFactory factory, string address, 
            Action<SecurityOption> configureSecurityOptions)
        {
            return factory.CreateChannel(address, new JTokenMessageParser(factory.Serializer), configureSecurityOptions);
        }

        public static IWampChannel<TMessage> CreateChannel<TMessage>(this IWampChannelFactory<TMessage> factory, string address, IWampTextMessageParser<TMessage> parser)
        {
            return factory.CreateChannel(address, parser, null);
        }

        public static IWampChannel<TMessage> CreateChannel<TMessage>(this IWampChannelFactory<TMessage> factory, string address, 
            IWampTextMessageParser<TMessage> parser, Action<SecurityOption> configureSecurityOptions)
        {
            return factory.CreateChannel(new WebSocket4NetTextConnection<TMessage>(address,
                new Wamp1Binding<TMessage>(parser, factory.Formatter), configureSecurityOptions));
        }

        public static IWampChannel<JToken> CreateChannel(this IWampChannelFactory<JToken> factory,
                                                         WebSocket socket)
        {
            return factory.CreateChannel(new WebSocket4NetTextConnection<JToken>(socket, new Wamp1Binding()));
        }
    }
}