using System.Collections.Generic;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Owin
{
    // Based on this sample:
    // http://aspnet.codeplex.com/sourcecontrol/latest#Samples/Katana/WebSocketSample/WebSocketServer/Startup.cs
    public abstract class WebSocketConnection<TMessage> : WebSocketWrapperConnection<TMessage>
    {
        public WebSocketConnection(IDictionary<string, object> webSocketContext, IWampStreamingMessageParser<TMessage> parser, ICookieProvider cookieProvider, ICookieAuthenticatorFactory cookieAuthenticatorFactory) : 
            base(new OwinWebSocketWrapper(webSocketContext), parser, cookieProvider, cookieAuthenticatorFactory)
        {
        }
    }
}