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
    public abstract class WebSocketWrapperConnection<TMessage> : AsyncWebSocketWampConnection<TMessage>
    {
        private readonly IWampStreamingMessageParser<TMessage> mParser;
        private readonly IWebSocketWrapper mWebSocket;
        private readonly CancellationTokenSource mCancellationTokenSource;
        private readonly Uri mAddressUri;
        private CancellationToken mCancellationToken;

        public WebSocketWrapperConnection(IWebSocketWrapper webSocket, IWampStreamingMessageParser<TMessage> parser, ICookieProvider cookieProvider, ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
            base(cookieProvider, cookieAuthenticatorFactory)
        {
            mWebSocket = webSocket;
            mParser = parser;
            mCancellationTokenSource = new CancellationTokenSource();
            mCancellationToken = mCancellationTokenSource.Token;
        }

        protected WebSocketWrapperConnection(IClientWebSocketWrapper clientWebSocket, Uri addressUri, string protocolName, IWampStreamingMessageParser<TMessage> parser) :
            this(clientWebSocket, parser, null, null)
        {
            clientWebSocket.Options.AddSubProtocol(protocolName);
            mAddressUri = addressUri;
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            mLogger.Debug("Sending a message");
            ArraySegment<byte> messageToSend = GetMessageInBytes(message);
            return mWebSocket.SendAsync(messageToSend, WebSocketMessageType, true, mCancellationToken);
        }

        protected abstract ArraySegment<byte> GetMessageInBytes(WampMessage<object> message);

        protected abstract WebSocketMessageType WebSocketMessageType { get; }

        protected async void Connect()
        {
            try
            {
                await this.ClientWebSocket.ConnectAsync(mAddressUri, mCancellationToken)
                          .ConfigureAwait(false);

                RaiseConnectionOpen();

                Task task = Task.Run((Func<Task>) this.RunAsync, mCancellationToken);
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
                RaiseConnectionClosed();
            }
        }

        public IClientWebSocketWrapper ClientWebSocket
        {
            get
            {
                return mWebSocket as IClientWebSocketWrapper;
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
                while (mWebSocket.IsConnected)
                {
                    // Reads data.
                    WebSocketReceiveResult webSocketReceiveResult;

                    long length = 0;
                    do
                    {
                        webSocketReceiveResult =
                            await mWebSocket.ReceiveAsync(receivedDataBuffer, mCancellationToken)
                                .ConfigureAwait(false);

                        length += webSocketReceiveResult.Count;

                        await memoryStream.WriteAsync(receivedDataBuffer.Array, receivedDataBuffer.Offset,
                                webSocketReceiveResult.Count, mCancellationToken)
                            .ConfigureAwait(false);

                    } while (!webSocketReceiveResult.EndOfMessage);

                    // If input frame is cancelation frame, send close command.
                    if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        try
                        {
                            await mWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                String.Empty, mCancellationToken)
                            .ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    }
                    else
                    {
                        memoryStream.Position = 0;
                        OnNewMessage(memoryStream);
                    }

                    memoryStream.Position = 0;
                    memoryStream.SetLength(0);
                }

                RaiseConnectionClosed();
            }
            catch (Exception ex)
            {
                //cancellation token could be cancelled id Dispose if a GoodBye message has been received.
                if (!(ex is OperationCanceledException))
                    RaiseConnectionError(ex);

                RaiseConnectionClosed();
            }
        }

        private void OnNewMessage(MemoryStream payloadData)
        {
            WampMessage<TMessage> message = mParser.Parse(payloadData);
            RaiseMessageArrived(message);
        }

        protected override void Dispose()
        {
            try
            {
                mCancellationTokenSource.Cancel();
                mCancellationTokenSource.Dispose();
            }
            catch (ObjectDisposedException)
            {
                // could be disposed multiple times, safe to swallow this.
            }
        }

        protected override bool IsConnected
        {
            get
            {
                return mWebSocket.IsConnected;
            }
        }
    }
}
