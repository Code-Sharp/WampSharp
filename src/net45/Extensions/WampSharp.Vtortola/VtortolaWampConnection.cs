using System;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Vtortola
{
    internal abstract class VtortolaWampConnection<TMessage> : IWampConnection<TMessage>
    {
        protected readonly WebSocket mWebsocket;

        public VtortolaWampConnection(WebSocket websocket)
        {
            mWebsocket = websocket;
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
            mWebsocket.Dispose();
        }

        public abstract void Send(WampMessage<TMessage> message);

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}