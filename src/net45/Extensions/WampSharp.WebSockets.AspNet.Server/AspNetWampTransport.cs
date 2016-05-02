using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;

namespace WampSharp.WebSockets
{
    public class AspNetWampTransport : WebSocketTransport<WebSocket>
    {
        public AspNetWampTransport(ICookieAuthenticatorFactory authenticatorFactory) : 
            base(authenticatorFactory)
        {
        }

        public async Task HandleRequest(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                // TODO: check: context.WebSockets.WebSocketRequestedProtocols
                WebSocket webSocket =
                    await context.WebSockets.AcceptWebSocketAsync(null)
                                 .ConfigureAwait(false);

                OnNewConnection(webSocket);
            }

            await next();
        }

        protected override void OpenConnection<TMessage>(IWampConnection<TMessage> connection)
        {
            WebSocketConnection<TMessage> casted = connection as WebSocketConnection<TMessage>;

            Task.Run(casted.RunAsync);
        }

        protected override string GetSubProtocol(WebSocket webSocket)
        {
            return webSocket.SubProtocol;
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (WebSocket webSocket,
             IWampBinaryBinding<TMessage> binding)
        {
            WebSocketConnection<TMessage> result =
                new BinaryWebSocketConnection<TMessage>(webSocket, binding);

            return result;
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocket webSocket,
             IWampTextBinding<TMessage> binding)
        {
            WebSocketConnection<TMessage> result =
                new TextWebSocketConnection<TMessage>(webSocket, binding);

            return result;
        }

        public override void Open()
        {
        }

        public override void Dispose()
        {
        }
    }
}