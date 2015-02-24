using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using vtortola.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Vtortola
{
    internal abstract class VtortolaWampConnection<TMessage> : IWampConnection<TMessage>
    {
        protected readonly WebSocket mWebsocket;
        private readonly ActionBlock<WampMessage<TMessage>> mSendBlock;

        protected VtortolaWampConnection(WebSocket websocket)
        {
            mWebsocket = websocket;

            mSendBlock = new ActionBlock<WampMessage<TMessage>>(x => InnerSend(x),
                new ExecutionDataflowBlockOptions() {MaxDegreeOfParallelism = 1});
        }

        public async Task HandleWebSocketAsync()
        {
            try
            {
                if (ConnectionOpen != null)
                {
                    ConnectionOpen(this, EventArgs.Empty);
                }

                while (mWebsocket.IsConnected)
                {
                    WebSocketMessageReadStream message =
                        await mWebsocket.ReadMessageAsync(CancellationToken.None)
                            .ConfigureAwait(false);

                    if (message != null)
                    {
                        using (message)
                        {
                            if (MessageArrived != null)
                            {
                                WampMessage<TMessage> parsed = ParseMessage(message);

                                MessageArrived(this,
                                    new WampMessageArrivedEventArgs<TMessage>(parsed));
                            }
                        }
                    }
                }

                if (ConnectionClosed != null)
                {
                    ConnectionClosed(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                if (ConnectionError != null)
                {
                    ConnectionError(this, new WampConnectionErrorEventArgs(ex));
                }
            }
        }

        protected abstract WampMessage<TMessage> ParseMessage(WebSocketMessageReadStream readStream);

        public void Dispose()
        {
            mSendBlock.Complete();
            mSendBlock.Completion.Wait();
            mWebsocket.Dispose();
        }

        public void Send(WampMessage<TMessage> message)
        {
            mSendBlock.Post(message);
        }

        private async Task InnerSend(WampMessage<TMessage> message)
        {
            if (mWebsocket.IsConnected)
            {
                try
                {
                    await SendAsync(message)
                        .ConfigureAwait(false);
                }
                catch
                {
                }                
            }
        }

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
        protected abstract Task SendAsync(WampMessage<TMessage> message);
    }
}