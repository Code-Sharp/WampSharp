using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;
using WampSharp.WebSockets;

namespace WampSharp.AspNet.WebSockets.Server
{
    /// <exclude />
    public sealed class AspNetWebsocketTransport : WebSocketTransport<WebSocketData>
    {
        private readonly string mUrl;
        private Route mRoute;

        /// <exclude />
        public AspNetWebsocketTransport(string url,
                                        ICookieAuthenticatorFactory authenticatorFactory = null)
            : base(authenticatorFactory)
        {
            mUrl = url;
        }

        /// <exclude />
        public override void Dispose()
        {
            if (mRoute != null)
            {
                RouteTable.Routes.Remove(mRoute);
            }
        }

        /// <exclude />
        protected override string GetSubProtocol(WebSocketData connection)
        {
            return connection.WebSocket.SubProtocol;
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
        (WebSocketData connection,
         IWampBinaryBinding<TMessage>
             binding)
        {
            return new BinaryWebSocketConnection<TMessage>(connection.WebSocket,
                                                           binding,
                                                           new AspNetCookieProvider(connection.HttpContext),
                                                           AuthenticatorFactory);
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
        (WebSocketData connection,
         IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketConnection<TMessage>(connection.WebSocket,
                                                         binding,
                                                         new AspNetCookieProvider(connection.HttpContext),
                                                         AuthenticatorFactory);
        }

        /// <exclude />
        public override void Open()
        {
            // Side effects, here we come :)
            mRoute = new Route(mUrl, new RouteHandler(this));
            RouteTable.Routes.Add(mUrl, mRoute);
        }

        private void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                IList<string> requestedProtocols = context.WebSocketRequestedProtocols;

                IEnumerable<string> possibleSubProtocols =
                    requestedProtocols.Intersect(SubProtocols);

                string subprotocol =
                    possibleSubProtocols.FirstOrDefault();

                if (subprotocol != null)
                {
                    context.AcceptWebSocketRequest(webSocketContext => ProcessWebSocket(webSocketContext, context),
                                                   new AspNetWebSocketOptions()
                                                   {
                                                       SubProtocol = subprotocol
                                                   });
                }
            }
        }

        private async Task ProcessWebSocket(AspNetWebSocketContext webSocketContext, HttpContext httpContext)
        {
            WebSocketData data = new WebSocketData(webSocketContext.WebSocket, httpContext);

            OnNewConnection(data);

            await data.ReadTask.ConfigureAwait(false);
        }

        /// <exclude />
        protected override void OpenConnection<TMessage>(WebSocketData original, IWampConnection<TMessage> connection)
        {
            WebSocketWrapperConnection<TMessage> casted = connection as WebSocketWrapperConnection<TMessage>;

            Task task = Task.Run(casted.RunAsync);

            original.ReadTask = task;
        }

        public class RouteHandler : IRouteHandler, IHttpHandler
        {
            private readonly AspNetWebsocketTransport mParent;

            public RouteHandler(AspNetWebsocketTransport parent)
            {
                mParent = parent;
            }

            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return this;
            }

            public void ProcessRequest(HttpContext context)
            {
                mParent.ProcessRequest(context);
            }

            public bool IsReusable
            {
                get
                {
                    return true;
                }
            }
        }
    }
}