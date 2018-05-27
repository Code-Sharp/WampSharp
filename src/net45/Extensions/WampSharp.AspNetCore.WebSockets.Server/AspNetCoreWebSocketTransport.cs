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
    public class AspNetCoreWebSocketTransport : WebSocketTransport<WebSocketData>
    {
        private Func<HttpContext, Func<Task>, Task> mHandler;

        public AspNetCoreWebSocketTransport
            (IApplicationBuilder app,
             ICookieAuthenticatorFactory authenticatorFactory = null) :
                 base(authenticatorFactory)
        {
            mHandler = this.EmptyHandler;
            app.Use(HttpHandler);
        }

        public override void Dispose()
        {
            mHandler = this.EmptyHandler;
        }

        protected override void OpenConnection<TMessage>(WebSocketData original, IWampConnection<TMessage> connection)
        {
            WebSocketWrapperConnection<TMessage> casted = connection as WebSocketWrapperConnection<TMessage>;

            Task task = Task.Run(casted.RunAsync);

            original.ReadTask = task;
        }

        protected override string GetSubProtocol(WebSocketData connection)
        {
            return connection.WebSocket.SubProtocol;
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (WebSocketData connection,
             IWampBinaryBinding<TMessage> binding)
        {
            return new BinaryWebSocketConnection<TMessage>
                (connection.WebSocket,
                 binding,
                 new AspNetCoreCookieProvider(connection.HttpContext),
                 AuthenticatorFactory);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocketData connection,
             IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketConnection<TMessage>
                (connection.WebSocket,
                 binding,
                 new AspNetCoreCookieProvider(connection.HttpContext),
                 AuthenticatorFactory);
        }

        public override void Open()
        {
            mHandler = this.WebSocketHandler;
        }

        private Task HttpHandler(HttpContext context, Func<Task> next)
        {
            return mHandler(context, next);
        }

        private async Task EmptyHandler(HttpContext context, Func<Task> next)
        {
            await next().ConfigureAwait(false);
        }

        private async Task WebSocketHandler(HttpContext context, Func<Task> next)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await next().ConfigureAwait(false);
            }
            else
            {
                IEnumerable<string> possibleSubProtocols =
                    context.WebSockets.WebSocketRequestedProtocols
                           .Intersect(this.SubProtocols);

                string subprotocol =
                    possibleSubProtocols.FirstOrDefault();

                if (subprotocol != null)
                {
                    using (var websocket =
                        await context.WebSockets.AcceptWebSocketAsync(subprotocol).ConfigureAwait(false))
                    {
                        // In an ideal world, OnNewConnection would return the
                        // connection itself and then we could somehow access its
                        // task, but for now we wrap the WebSocket with a WebSocketData
                        // struct, and let OnNewConnection to fill us magically
                        // the ReadTask
                        WebSocketData webSocketData = new WebSocketData(websocket, context);

                        OnNewConnection(webSocketData);

                        await webSocketData.ReadTask.ConfigureAwait(false);
                    }
                }
            }
        }
    }
}