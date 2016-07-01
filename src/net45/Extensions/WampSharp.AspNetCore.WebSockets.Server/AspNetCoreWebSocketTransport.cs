using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;
using WampSharp.WebSockets;

namespace WampSharp.AspNetCore.WebSockets.Server
{
    public class AspNetCoreWebSocketTransport : WebSocketTransport<WebSocket>
    {
        private readonly IApplicationBuilder mApp;

        public AspNetCoreWebSocketTransport
            (IApplicationBuilder app,
             ICookieAuthenticatorFactory authenticatorFactory) :
                 base(authenticatorFactory)
        {
            mApp = app;
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        protected override void OpenConnection<TMessage>(IWampConnection<TMessage> connection)
        {
            WebSocketConnection<TMessage> casted = connection as WebSocketConnection<TMessage>;

            Task.Run(casted.RunAsync);
        }

        protected override string GetSubProtocol(WebSocket connection)
        {
            return connection.SubProtocol;
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (WebSocket connection,
             IWampBinaryBinding<TMessage> binding)
        {
            return new BinaryWebSocketConnection<TMessage>(connection, binding);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocket connection,
             IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketConnection<TMessage>(connection, binding);
        }

        public override void Open()
        {
            mApp.Use(HttpHandler);
        }

        private async Task HttpHandler(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                // TODO: Think of a better way to do this.
                IEnumerable<Task<WebSocket>> tasks =
                    this.SubProtocols
                        .Select(x => AcceptWebSocket(context, x));

                Task<WebSocket> websocketTask =
                    await Task.WhenAny(tasks).ConfigureAwait(false);

                WebSocket websocket = 
                    await websocketTask.ConfigureAwait(false);

                OnNewConnection(websocket);

                // TODO: Check how to leave the connection open
                //await Task.Delay(TimeSpan.FromSeconds(120));

                return;
            }

            await next();
        }

        private static Task<WebSocket> AcceptWebSocket(HttpContext context, string subProtocol)
        {
            return context.WebSockets.AcceptWebSocketAsync(subProtocol);
        }
    }
}