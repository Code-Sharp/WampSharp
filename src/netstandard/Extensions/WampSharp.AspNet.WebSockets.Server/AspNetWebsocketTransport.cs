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
    public sealed class AspNetWebSocketTransport : WebSocketTransport<WebSocketData>
    {
        private readonly string mUrl;
        private readonly object mLock = new object();
        private Route mRoute;
        private readonly int? mMaxFrameSize;

        /// <exclude />
        public AspNetWebSocketTransport(string url,
                                        ICookieAuthenticatorFactory authenticatorFactory = null,
                                        int? maxFrameSize = null)
            : base(authenticatorFactory)
        {
            mUrl = url;
            mMaxFrameSize = maxFrameSize;
        }

        /// <exclude />
        public override void Dispose()
        {
            lock (mLock)
            {
                if (mRoute != null)
                {
                    RouteTable.Routes.Remove(mRoute);
                    mRoute = null;
                }
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
                                                           AuthenticatorFactory,
                                                           mMaxFrameSize);
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
        (WebSocketData connection,
         IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketConnection<TMessage>(connection.WebSocket,
                                                         binding,
                                                         new AspNetCookieProvider(connection.HttpContext),
                                                         AuthenticatorFactory,
                                                         mMaxFrameSize);
        }

        /// <exclude />
        public override void Open()
        {
            lock (mLock)
            {
                // Side effects, here we come :)
                mRoute = new Route(mUrl, new RouteHandler(this));
                RouteTable.Routes.Add(mUrl, mRoute);
            }
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
            IWampWebSocketWrapperConnection casted = connection as IWampWebSocketWrapperConnection;

            Task task = Task.Run(casted.RunAsync);

            original.ReadTask = task;
        }

        private class RouteHandler : IRouteHandler, IHttpHandler
        {
            private readonly AspNetWebSocketTransport mParent;

            public RouteHandler(AspNetWebSocketTransport parent)
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

            public bool IsReusable => true;
        }
    }
}