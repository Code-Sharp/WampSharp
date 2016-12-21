using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;
using WampSharp.WebSockets;

namespace WampSharp.AspNet.WebSockets.Server
{
    /// <exclude />
    public sealed class AspNetWebsocketTransform : WebSocketTransport<WebSocketData>
    {
        /// <exclude />
        public AspNetWebsocketTransform(ICookieAuthenticatorFactory authenticatorFactory = null) : base(authenticatorFactory)
        {
        }

        /// <exclude />
        public override void Dispose()
        {
        }

        /// <exclude />
        protected override void OpenConnection<TMessage>(WebSocketData original, IWampConnection<TMessage> connection)
        {
            WebSocketConnection<TMessage> casted = connection as WebSocketConnection<TMessage>;

            Task task = Task.Run(casted.RunAsync);

            original.ReadTask = task;
        }

        /// <exclude />
        protected override string GetSubProtocol(WebSocketData connection)
        {
            return connection.WebSocket.SubProtocol;
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>(WebSocketData connection, IWampBinaryBinding<TMessage> binding)
        {
            return new BinaryWebSocketConnection<TMessage>(connection.WebSocket,
                binding,
                new AspNetCookieProvider(connection.HttpContext),
                AuthenticatorFactory);
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>(WebSocketData connection, IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketConnection<TMessage>(connection.WebSocket,
                binding,
                new AspNetCookieProvider(connection.HttpContext),
                AuthenticatorFactory);
        }

        /// <exclude />
        public override void Open()
        {
        }

        /// <exclude />
        public async Task NewConnection(WebSocketData data)
        {
            OnNewConnection(data);
            await data.ReadTask.ConfigureAwait(false);
        }

        /// <exclude />
        public class RouterController : ApiController
        {
            private readonly AspNetWebsocketTransform _aspNetWebsocketTransform;

            /// <exclude />
            public RouterController(AspNetWebsocketTransform aspNetWebsocketTransform)
            {
                _aspNetWebsocketTransform = aspNetWebsocketTransform;
            }

            /// <exclude />
            public virtual HttpResponseMessage Get()
            {
                if (HttpContext.Current.IsWebSocketRequest)
                {
                    IEnumerable<string> possibleSubProtocols =
                        HttpContext.Current.WebSocketRequestedProtocols
                            .Intersect(_aspNetWebsocketTransform.SubProtocols);

                    string subprotocol =
                        possibleSubProtocols.FirstOrDefault();

                    if (subprotocol != null)
                    {
                        HttpContext.Current.AcceptWebSocketRequest(async (webSocketContext) =>
                        {
                            var webSocketData = new WebSocketData(webSocketContext.WebSocket, HttpContext.Current);
                            await _aspNetWebsocketTransform.NewConnection(webSocketData);
                        },
                            new AspNetWebSocketOptions()
                            {
                                SubProtocol = HttpContext.Current.WebSocketRequestedProtocols.FirstOrDefault()
                            });
                    }
                    return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
