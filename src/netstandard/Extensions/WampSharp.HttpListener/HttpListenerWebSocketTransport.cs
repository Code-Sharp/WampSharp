using System;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Transports;
using WampSharp.WebSockets;

namespace WampSharp.HttpListener
{
    /// <exclude />
    public sealed class HttpListenerWebSocketTransport : WebSocketTransport<WebSocketData>
    {
        private const string SecWebSocketProtocolHeader = "Sec-WebSocket-Protocol";
        private readonly string mUrl;
        private readonly Action<HttpListenerContext> mOnUnknownRequest;
        private System.Net.HttpListener mHttpListener;
        private int? mMaxFrameSize;

        /// <exclude />
        public HttpListenerWebSocketTransport
        (string url,
         Action<HttpListenerContext> onUnknownRequest = null,
         ICookieAuthenticatorFactory authenticatorFactory = null,
         int? maxFrameSize = null)
            : base(authenticatorFactory)
        {
            mUrl = url;
            mMaxFrameSize = maxFrameSize;
            mOnUnknownRequest = onUnknownRequest ?? CancelRequest;
        }

        private void CancelRequest(HttpListenerContext context)
        {
            context.Response.Abort();
        }

        /// <exclude />
        public override void Dispose()
        {
            mHttpListener.Stop();
        }

        /// <exclude />
        private string GetSubProtocol(HttpListenerContext context)
        {
            string[] requestedSubprocols =
                context.Request.Headers[SecWebSocketProtocolHeader].Split(',');

            return requestedSubprocols.Intersect(this.SubProtocols).FirstOrDefault();
        }

        /// <exclude />
        protected override string GetSubProtocol(WebSocketData connection)
        {
            return connection.SubProtocol;
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateBinaryConnection<TMessage>
        (WebSocketData connection,
         IWampBinaryBinding<TMessage>
             binding)
        {
            return new BinaryWebSocketConnection<TMessage>(connection.Context.WebSocket,
                                                           binding,
                                                           new HttpListenerCookieProvider(connection.Context), 
                                                           AuthenticatorFactory,
                                                           mMaxFrameSize);
        }

        /// <exclude />
        protected override IWampConnection<TMessage> CreateTextConnection<TMessage>
        (WebSocketData connection,
         IWampTextBinding<TMessage> binding)
        {
            return new TextWebSocketConnection<TMessage>(connection.Context.WebSocket,
                                                         binding,
                                                         new HttpListenerCookieProvider(connection.Context),
                                                         AuthenticatorFactory,
                                                         mMaxFrameSize);
        }

        /// <exclude />
        public override void Open()
        {
            mHttpListener = new System.Net.HttpListener();
            mHttpListener.Prefixes.Add(mUrl);
            mHttpListener.Start();

            Task.Run(ListenAsync);
        }

        private async Task ListenAsync()
        {
            while (mHttpListener.IsListening)
            {
                HttpListenerContext context =
                    await mHttpListener.GetContextAsync().ConfigureAwait(false);

                if (!context.Request.IsWebSocketRequest)
                {
                    OnUnknownRequest(context);
                }
                else
                {
                    string subProtocol = GetSubProtocol(context);

                    if (subProtocol == null)
                    {
                        OnUnknownRequest(context);
                    }
                    else
                    {
                        HttpListenerWebSocketContext webSocketContext =
                            await context.AcceptWebSocketAsync(subProtocol);

                        WebSocketData webSocketData = new WebSocketData(webSocketContext, subProtocol);

                        OnNewConnection(webSocketData);
                    }
                }
            }
        }

        private void OnUnknownRequest(HttpListenerContext context)
        {
            mOnUnknownRequest(context);
        }

        /// <exclude />
        protected override void OpenConnection<TMessage>(WebSocketData original, IWampConnection<TMessage> connection)
        {
            IWampWebSocketWrapperConnection casted = connection as IWampWebSocketWrapperConnection;

            Task task = Task.Run(casted.RunAsync);
        }
    }
}