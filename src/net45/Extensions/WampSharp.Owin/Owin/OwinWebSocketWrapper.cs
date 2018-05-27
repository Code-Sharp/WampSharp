using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.WebSockets;

namespace WampSharp.Owin
{
    internal class OwinWebSocketWrapper : IWebSocketWrapper
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

        internal OwinWebSocketWrapper(IDictionary<string, object> websocketContext)
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

                if (mWebsocketContext.TryGetValue(WebSocketClientCloseDescription, out object description))
                {
                    return (string)description;
                }

                return null;
            }
        }

        public WebSocketCloseStatus? ClientCloseStatus
        {
            get
            {

                if (mWebsocketContext.TryGetValue(WebSocketClientCloseStatus, out object status))
                {
                    return (WebSocketCloseStatus)(int)status;
                }

                return null;
            }
        }

        public CancellationToken CancellationToken => (CancellationToken) mWebsocketContext[WebSocketCallCancelled];

        public string SubProtocol
        {
            get
            {

                if (mWebsocketContext.TryGetValue(WebSocketSubProtocol, out object subprotocol))
                {
                    return (string)subprotocol;
                }

                return null;
            }
        }


        public async Task<WebSocketReceiveResult> ReceiveAsync
        (ArraySegment<byte> arraySegment,
         CancellationToken callCancelled)
        {
            Tuple<int, bool, int> result =
                await mReceiveAsync(arraySegment, callCancelled)
                    .ConfigureAwait(false);

            WebSocketMessageType webSocketMessageType = GetMessageType(result.Item1);

            if (webSocketMessageType == WebSocketMessageType.Close)
            {
                ChangeState(actionDone: WebSocketState.CloseReceived,
                            dualAction: WebSocketState.CloseSent);

                return new WebSocketReceiveResult(count: result.Item3,
                                                  messageType: webSocketMessageType,
                                                  endOfMessage: result.Item2,
                                                  closeStatus: this.ClientCloseStatus,
                                                  closeStatusDescription: WebSocketClientCloseDescription);
            }

            return new WebSocketReceiveResult(count: result.Item3, messageType: webSocketMessageType, endOfMessage: result.Item2);
        }

        public WebSocketState State { get; private set; } = WebSocketState.Open;

        public Task SendAsync(ArraySegment<byte> data, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancel)
        {
            return mSendAsync(data, ConvertMessageTypeToInt(messageType), endOfMessage, cancel);
        }

        public Task CloseAsync(WebSocketCloseStatus closeStatus, string closeDescription, CancellationToken cancel)
        {
            ChangeState(actionDone: WebSocketState.CloseSent,
                        dualAction: WebSocketState.CloseReceived);

            return mCloseAsync((int) closeStatus, closeDescription, cancel);
        }

        private void ChangeState(WebSocketState actionDone, WebSocketState dualAction)
        {
            if (State == WebSocketState.Open)
            {
                State = actionDone;
            }
            else if (State == dualAction)
            {
                State = WebSocketState.Closed;
            }
        }

        private WebSocketMessageType GetMessageType(int messageType)
        {
            switch (messageType)
            {
                case WebSocketMessageTypes.Binary:
                    return WebSocketMessageType.Binary;
                case WebSocketMessageTypes.Text:
                    return WebSocketMessageType.Text;
                case WebSocketMessageTypes.Close:
                    return WebSocketMessageType.Close;
            }

            return default(WebSocketMessageType);
        }

        private int ConvertMessageTypeToInt(WebSocketMessageType messageType)
        {
            switch (messageType)
            {
                case WebSocketMessageType.Binary:
                    return WebSocketMessageTypes.Binary;
                case WebSocketMessageType.Text:
                    return WebSocketMessageTypes.Text;
                case WebSocketMessageType.Close:
                    return WebSocketMessageTypes.Close;
            }

            return 0;
        }
    }
}