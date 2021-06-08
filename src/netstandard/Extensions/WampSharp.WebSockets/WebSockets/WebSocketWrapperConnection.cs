using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Logging;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.WebSockets
{
    // Based on this sample:
    // https://code.msdn.microsoft.com/vstudio/The-simple-WebSocket-4524921c
    public abstract class WebSocketWrapperConnection<TMessage, TRaw> : AsyncWebSocketWampConnection<TMessage>, IWampWebSocketWrapperConnection
    {
        private readonly IWampMessageParser<TMessage, TRaw> mParser;
        private readonly IWebSocketWrapper mWebSocket;
        private CancellationTokenSource mCancellationTokenSource;
        private readonly Uri mAddressUri;
        private readonly CancellationToken mCancellationToken;
        private readonly int? mMaxFrameSize;

        public WebSocketWrapperConnection(IWebSocketWrapper webSocket, IWampMessageParser<TMessage, TRaw> parser,
                                          ICookieProvider cookieProvider,
                                          ICookieAuthenticatorFactory cookieAuthenticatorFactory, int? maxFrameSize) :
            base(cookieProvider, cookieAuthenticatorFactory)
        {
            mWebSocket = webSocket;
            mParser = parser;
            mMaxFrameSize = maxFrameSize;
            mCancellationTokenSource = new CancellationTokenSource();
            mCancellationToken = mCancellationTokenSource.Token;
        }

        protected WebSocketWrapperConnection(IClientWebSocketWrapper clientWebSocket, Uri addressUri,
                                             string protocolName, IWampMessageParser<TMessage, TRaw> parser,
                                             int? maxFrameSize) :
            this(clientWebSocket, parser, null, null, maxFrameSize)
        {
            clientWebSocket.Options.AddSubProtocol(protocolName);
            mAddressUri = addressUri;
        }

        protected override async Task SendAsync(WampMessage<object> message)
        {
            mLogger.Debug("Attempting to send a message");
            ArraySegment<byte> messageToSend = GetMessageInBytes(message);

            if (mMaxFrameSize == null)
            {
                await mWebSocket.SendAsync(messageToSend, WebSocketMessageType, true, mCancellationToken)
                                .ConfigureAwait(false);
            }
            else
            {
                int maxFrameSize = mMaxFrameSize.Value;

                int remainingSize = messageToSend.Count;

                bool endOfMessage = false;
                int currentFrameSize = maxFrameSize;
                int offset = 0;

                while (remainingSize > 0)
                {
                    if (remainingSize < maxFrameSize)
                    {
                        endOfMessage = true;
                        currentFrameSize = remainingSize;
                    }

                    ArraySegment<byte> partialMessage = new ArraySegment<byte>(messageToSend.Array, 
                                                                               offset,
                                                                               currentFrameSize);

                    await mWebSocket.SendAsync(partialMessage, WebSocketMessageType, endOfMessage, mCancellationToken)
                                    .ConfigureAwait(false);

                    offset += maxFrameSize;
                    remainingSize -= maxFrameSize;
                }
            }
        }

        private ArraySegment<byte> GetMessageInBytes(WampMessage<object> message)
        {
            byte[] bytes = mParser.GetBytes(message);
            return new ArraySegment<byte>(bytes);
        }

        protected abstract WebSocketMessageType WebSocketMessageType { get; }

        protected async void InnerConnect()
        {
            bool connected = await TryConnect();

            if (connected)
            {
                await Task.Run(this.RunAsync, mCancellationToken).ConfigureAwait(false);
            }
        }

        private async Task<bool> TryConnect()
        {
            try
            {
                await this.ClientWebSocket.ConnectAsync(mAddressUri, mCancellationToken)
                          .ConfigureAwait(false);

                RaiseConnectionOpen();

                return true;
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
                RaiseConnectionClosed();
            }

            return false;
        }

        public IClientWebSocketWrapper ClientWebSocket => mWebSocket as IClientWebSocketWrapper;

        public async Task RunAsync()
        {
            try
            {
                /*We define a certain constant which will represent
                  size of received data. It is established by us and 
                  we can set any value. We know that in this case the size of the sent
                  data is very small.
                */
                const int maxMessageSize = 2048;

                // Buffer for received bits.
                ArraySegment<byte> receivedDataBuffer = new ArraySegment<byte>(new byte[maxMessageSize]);

                MemoryStream memoryStream = new MemoryStream();

                // Checks WebSocket state.
                while (IsConnected && !mCancellationToken.IsCancellationRequested)
                {
                    // Reads data.
                    WebSocketReceiveResult webSocketReceiveResult =
                        await ReadMessage(receivedDataBuffer, memoryStream).ConfigureAwait(false);

                    if (webSocketReceiveResult.MessageType != WebSocketMessageType.Close)
                    {
                        memoryStream.Position = 0;
                        OnNewMessage(memoryStream);
                    }

                    memoryStream.Position = 0;
                    memoryStream.SetLength(0);
                }
            }
            catch (Exception ex)
            {
                // Cancellation token could be cancelled in Dispose if a 
                // Goodbye message has been received.
                if (!(ex is OperationCanceledException) ||
                    !mCancellationToken.IsCancellationRequested)
                {
                    RaiseConnectionError(ex);
                }
            }

            if (mWebSocket.State != WebSocketState.CloseReceived &&
                mWebSocket.State != WebSocketState.Closed)
            {
                await CloseWebSocket().ConfigureAwait(false);
            }

            RaiseConnectionClosed();
        }

        private async Task CloseWebSocket()
        {
            try
            {
                await mWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                            String.Empty,
                                            CancellationToken.None)
                                .ConfigureAwait(false);
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.InvalidState)
            {
                mLogger.DebugException("Failed closing the websocket as it was already aborted", ex);
            }
            catch (Exception ex)
            {
                mLogger.WarnException("Failed sending a close message to client", ex);
            }
        }

        private async Task<WebSocketReceiveResult> ReadMessage(ArraySegment<byte> receivedDataBuffer, MemoryStream memoryStream)
        {
            WebSocketReceiveResult webSocketReceiveResult;

            long length = 0;

            do
            {
                webSocketReceiveResult =
                    await mWebSocket.ReceiveAsync(receivedDataBuffer, mCancellationToken)
                                    .ConfigureAwait(false);

                length += webSocketReceiveResult.Count;

                await memoryStream.WriteAsync(receivedDataBuffer.Array,
                                              receivedDataBuffer.Offset,
                                              webSocketReceiveResult.Count,
                                              mCancellationToken)
                                  .ConfigureAwait(false);
            }
            while (!webSocketReceiveResult.EndOfMessage);

            return webSocketReceiveResult;
        }

        private void OnNewMessage(MemoryStream payloadData)
        {
            WampMessage<TMessage> message = mParser.Parse(payloadData);
            RaiseMessageArrived(message);
        }

        protected override void Dispose()
        {
            mCancellationTokenSource.Cancel();
            mCancellationTokenSource.Dispose();
            mCancellationTokenSource = null;
        }

        protected override bool IsConnected => mWebSocket.State == WebSocketState.Open;
    }
}
