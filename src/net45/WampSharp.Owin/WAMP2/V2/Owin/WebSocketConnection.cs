using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.Owin
{
    // Based on this sample:
    // http://aspnet.codeplex.com/sourcecontrol/latest#Samples/Katana/WebSocketSample/WebSocketServer/Startup.cs
    // TODO: abstractize this, in order to avoid code duplication
    public abstract class WebSocketConnection<TMessage> : AsyncWebSocketWampConnection<TMessage>
    {
        private const int NormalClosure = 1000;
        private readonly IWampStreamingMessageParser<TMessage> mParser;
        private readonly WebSocketWrapper mWebSocket;
        private readonly CancellationTokenSource mCancellationTokenSource;

        public WebSocketConnection(IDictionary<string, object> webSocketContext, IWampStreamingMessageParser<TMessage> parser, ICookieProvider cookieProvider, ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
            base(cookieProvider, cookieAuthenticatorFactory)
        {
            mWebSocket = new WebSocketWrapper(webSocketContext);
            mParser = parser;
            mCancellationTokenSource = new CancellationTokenSource();
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            ArraySegment<byte> messageToSend = GetMessageInBytes(message);
            return mWebSocket.SendAsync(messageToSend, MessageType, true, mCancellationTokenSource.Token);
        }

        protected abstract ArraySegment<byte> GetMessageInBytes(WampMessage<object> message);

        protected abstract int MessageType { get; }

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
                while (IsConnected)
                {
                    // Reads data.
                    WebSocketReceiveResultStruct webSocketReceiveResult;

                    long length = 0;
                    do
                    {
                        webSocketReceiveResult =
                            await mWebSocket.ReceiveAsync(receivedDataBuffer, mCancellationTokenSource.Token)
                            .ConfigureAwait(false);

                        length += webSocketReceiveResult.Count;

                        await memoryStream.WriteAsync(receivedDataBuffer.Array, receivedDataBuffer.Offset,
                                                      webSocketReceiveResult.Count, mCancellationTokenSource.Token)
                                          .ConfigureAwait(false);

                    } while (!webSocketReceiveResult.EndOfMessage);

                    // If input frame is cancelation frame, send close command.
                    if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        this.RaiseConnectionClosed();

                        await mWebSocket.CloseAsync(NormalClosure,
                                                    String.Empty, 
                                                    mCancellationTokenSource.Token)
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
                return mWebSocket.ClientCloseStatus == null ||
                       mWebSocket.ClientCloseStatus == 0;
            }
        }
    }
}