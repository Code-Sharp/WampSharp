using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.WebSockets
{
    // Based on this sample:
    // https://code.msdn.microsoft.com/vstudio/The-simple-WebSocket-4524921c
    public abstract class WebSocketConnection<TMessage> : AsyncWebSocketWampConnection<TMessage>
    {
        private readonly IWampStreamingMessageParser<TMessage> mParser;
        private readonly WebSocket mWebSocket;
        private readonly CancellationTokenSource mCancellationTokenSource;
        private readonly Uri mAddressUri;

        public WebSocketConnection(WebSocket webSocket, IWampStreamingMessageParser<TMessage> parser, ICookieProvider cookieProvider, ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
            base(cookieProvider, cookieAuthenticatorFactory)
        {
            mWebSocket = webSocket;
            mParser = parser;
            mCancellationTokenSource = new CancellationTokenSource();
        }

        protected WebSocketConnection(Uri addressUri, string protocolName, IWampStreamingMessageParser<TMessage> parser) :
            this(new ClientWebSocket(), parser, null, null)
        {
            ClientWebSocket.Options.AddSubProtocol(protocolName);
            mAddressUri = addressUri;
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            ArraySegment<byte> messageToSend = GetMessageInBytes(message);
            return mWebSocket.SendAsync(messageToSend, WebSocketMessageType, true, mCancellationTokenSource.Token);
        }

        protected abstract ArraySegment<byte> GetMessageInBytes(WampMessage<object> message);

        protected abstract WebSocketMessageType WebSocketMessageType { get; }

        protected async void Connect()
        {
            try
            {
                await this.ClientWebSocket.ConnectAsync(mAddressUri, mCancellationTokenSource.Token)
                          .ConfigureAwait(false);

                RaiseConnectionOpen();

                Task task = Task.Run(this.RunAsync, mCancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
                RaiseConnectionClosed();
            }
        }

        public ClientWebSocket ClientWebSocket
        {
            get
            {
                return mWebSocket as ClientWebSocket;
            }
        }

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
                while (mWebSocket.State == WebSocketState.Open)
                {
                    // Reads data.
                    WebSocketReceiveResult webSocketReceiveResult;

                    long length = 0;
                    do
                    {
                        webSocketReceiveResult =
                            await mWebSocket.ReceiveAsync(receivedDataBuffer, mCancellationTokenSource.Token)
                            .ConfigureAwait(false);

                        length += webSocketReceiveResult.Count;

                        await memoryStream.WriteAsync(receivedDataBuffer.Array, receivedDataBuffer.Offset,
                                                      receivedDataBuffer.Count, mCancellationTokenSource.Token)
                                          .ConfigureAwait(false);

                    } while (!webSocketReceiveResult.EndOfMessage);

                    // If input frame is cancelation frame, send close command.
                    if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        this.RaiseConnectionClosed();

                        await mWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                                    String.Empty, mCancellationTokenSource.Token)
                                        .ConfigureAwait(false);
                    }
                    else
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
                RaiseConnectionError(ex);
            }
        }

        private void OnNewMessage(MemoryStream payloadData)
        {
            WampMessage<TMessage> message = mParser.Parse(payloadData);
            RaiseMessageArrived(message);
        }

        protected override void Dispose()
        {
            mCancellationTokenSource.Cancel();
        }

        protected override bool IsConnected
        {
            get
            {
                return mWebSocket.State == WebSocketState.Open;
            }
        }
    }
}