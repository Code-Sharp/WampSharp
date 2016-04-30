using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using Websocket = System.Net.WebSockets.WebSocket;

namespace WampSharp.WebSocket
{
    // Based on this sample:
    // https://code.msdn.microsoft.com/vstudio/The-simple-WebSocket-4524921c
    public abstract class WebSocketConnection<TMessage> : AsyncWampConnection<TMessage>
    {
        private readonly Websocket mWebSocket;
        protected readonly CancellationTokenSource mCancellationTokenSource;

        public WebSocketConnection(Websocket webSocket)
        {
            mWebSocket = webSocket;
            mCancellationTokenSource = new CancellationTokenSource();
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            ArraySegment<byte> messageToSend = GetMessageInBytes(message);
            return mWebSocket.SendAsync(messageToSend, WebSocketMessageType, true, mCancellationTokenSource.Token);
        }

        protected abstract ArraySegment<byte> GetMessageInBytes(WampMessage<object> message);

        protected abstract WebSocketMessageType WebSocketMessageType { get; }

        protected async Task RunAsync()
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
                        memoryStream.SetLength(length);
                        OnNewMessage(memoryStream);

                        memoryStream.Position = 0;
                        memoryStream.SetLength(0);
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
            }
        }

        protected abstract void OnNewMessage(MemoryStream payloadData);

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