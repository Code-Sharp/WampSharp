using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;
using WampSharp.WebSockets;

namespace WampSharp.Owin
{
    public class OwinWebSocketTransport : WebSocketTransport<WebSocketData>
    {
        private const string WebSocketSubProtocol = "websocket.SubProtocol";
        private const string SecWebSocketProtocolHeader = "Sec-WebSocket-Protocol";

        private Func<IOwinContext, Func<Task>, Task> mHandler;
        private int? mMaxFrameSize;

        public OwinWebSocketTransport
        (IAppBuilder app,
         int? maxFrameSize,
         ICookieAuthenticatorFactory authenticatorFactory = null) :
                 base(authenticatorFactory)
        {
            mHandler = this.EmptyHandler;
            app.Use(HttpHandler);
            mMaxFrameSize = maxFrameSize;
        }

        public override void Dispose()
        {
            mHandler = this.EmptyHandler;
        }

        protected override void OpenConnection<TMessage>(WebSocketData original, IWampConnection<TMessage> connection)
        {
            IWampWebSocketWrapperConnection casted = connection as IWampWebSocketWrapperConnection;

            Task task = Task.Run(casted.RunAsync);

            original.ReadTask = task;
        }

        protected override string GetSubProtocol(WebSocketData connection)
        {
            return connection.SubProtocol;
        }

        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
            (WebSocketData connection,
             IWampBinaryBinding<TMessage> binding)
        {
            return new BinaryWebSocketWrapperConnection<TMessage>
                (new OwinWebSocketWrapper(connection.WebSocketContext),
                 binding,
                 new OwinCookieProvider(connection.OwinContext),
                 AuthenticatorFactory,
                 mMaxFrameSize);
        }

        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
            (WebSocketData connection,
             IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketWrapperConnection<TMessage>
                (new OwinWebSocketWrapper(connection.WebSocketContext),
                 binding,
                 new OwinCookieProvider(connection.OwinContext),
                 AuthenticatorFactory,
                 mMaxFrameSize);
        }

        public override void Open()
        {
            mHandler = this.WebSocketHandler;
        }

        private Task HttpHandler(IOwinContext context, Func<Task> next)
        {
            return mHandler(context, next);
        }

        private async Task EmptyHandler(IOwinContext owinContext, Func<Task> next)
        {
            await next().ConfigureAwait(false);
        }

        private async Task WebSocketHandler(IOwinContext context, Func<Task> next)
        {
            Action<IDictionary<string, object>, Func<IDictionary<string, object>, Task>> accept = 
                context.Get<Action<IDictionary<string, object>, Func<IDictionary<string, object>, Task>>>("websocket.Accept");

            if (accept != null)
            {
                IEnumerable<string> possibleSubProtocols =
                    context.Request.Headers.GetCommaSeparatedValues(SecWebSocketProtocolHeader)
                           .Intersect(this.SubProtocols);

                string subprotocol =
                    possibleSubProtocols.FirstOrDefault();

                if (subprotocol != null)
                {
                    accept(new Dictionary<string, object>()
                           {
                               {WebSocketSubProtocol, subprotocol}
                           },
                           websocketContext => OnAccept(websocketContext, context, subprotocol));

                    return;
                }
            }

            await next().ConfigureAwait(false);
        }

        private async Task OnAccept(IDictionary<string, object> webSocketContext, IOwinContext context, string subprotocol)
        {
            // In an ideal world, OnNewConnection would return the
            // connection itself and then we could somehow access its
            // task, but for now we wrap the WebSocket with a WebSocketData
            // struct, and let OnNewConnection to fill us magically
            // the ReadTask
            WebSocketData webSocketData = new WebSocketData(webSocketContext, context, subprotocol);

            OnNewConnection(webSocketData);

            await webSocketData.ReadTask.ConfigureAwait(false);
        }
    }
}