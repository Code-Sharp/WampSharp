using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.Owin
{
    internal class WebSocketWrapper
    {
        private readonly IDictionary<string, object> mWebsocketContext;
        private readonly Func<ArraySegment<byte>, int, bool, CancellationToken, Task> mSendAsync;
        private readonly Func<ArraySegment<byte>, CancellationToken, Task<Tuple<int, bool, int>>> mReceiveAsync;
        private readonly Func<int, string, CancellationToken, Task> mCloseAsync;

        private const string WebSocketSendAsync = "websocket.SendAsync";
        private const string WebSocketReceiveAsync = "websocket.ReceiveAsync";
        private const string WebSocketCloseAsync = "websocket.CloseAsync";
        private const string WebSocketCallCancelled = "websocket.CallCancelled";
        private const string WebSocketClientCloseDescription = "websocket.ClientCloseDescription";
        private const string WebSocketClientCloseStatus = "websocket.ClientCloseStatus";
        private const string WebSocketSubProtocol = "websocket.SubProtocol";

        internal WebSocketWrapper(IDictionary<string, object> websocketContext)
        {
            mWebsocketContext = websocketContext;

            mSendAsync =
                (Func<ArraySegment<byte>, int, bool, CancellationToken, Task>) mWebsocketContext[WebSocketSendAsync];

            mReceiveAsync =
                (Func<ArraySegment<byte>, CancellationToken, Task<Tuple<int, bool, int>>>)
                mWebsocketContext[WebSocketReceiveAsync];

            mCloseAsync = (Func<int, string, CancellationToken, Task>) mWebsocketContext[WebSocketCloseAsync];
        }

        public string ClientCloseDescription
        {
            get
            {
                object description;

                if (mWebsocketContext.TryGetValue(WebSocketClientCloseDescription, out description))
                {
                    return (string) description;
                }

                return null;
            }
        }

        public int? ClientCloseStatus
        {
            get
            {
                object status;

                if (mWebsocketContext.TryGetValue(WebSocketClientCloseStatus, out status))
                {
                    return (int) status;
                }

                return null;
            }
        }

        public CancellationToken CancellationToken
        {
            get { return (CancellationToken) mWebsocketContext[WebSocketCallCancelled]; }
        }

        public string SubProtocol
        {
            get
            {
                object subprotocol;

                if (mWebsocketContext.TryGetValue(WebSocketSubProtocol, out subprotocol))
                {
                    return (string) subprotocol;
                }

                return null;
            }
        }


        public async Task<WebSocketReceiveResultStruct> ReceiveAsync
        (ArraySegment<byte> arraySegment,
         CancellationToken callCancelled)
        {
            Tuple<int, bool, int> result =
                await mReceiveAsync(arraySegment, callCancelled)
                    .ConfigureAwait(false);

            return new WebSocketReceiveResultStruct
            {
                Count = result.Item3,
                EndOfMessage = result.Item2,
                MessageType = result.Item1
            };
        }

        public Task SendAsync(ArraySegment<byte> data, int messageType, bool endOfMessage, CancellationToken cancel)
        {
            return mSendAsync(data, messageType, endOfMessage, cancel);
        }

        public Task CloseAsync(int closeStatus, string closeDescription, CancellationToken cancel)
        {
            return mCloseAsync(closeStatus, closeDescription, cancel);
        }
    }
}