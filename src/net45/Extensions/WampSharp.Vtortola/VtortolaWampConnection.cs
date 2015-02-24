using System;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Vtortola
{
    internal abstract class VtortolaWampConnection<TMessage> : AsyncWampConnection<TMessage>, IWampConnection<TMessage>
    {
        protected readonly WebSocket mWebsocket;

        protected VtortolaWampConnection(WebSocket websocket)
        {
            mWebsocket = websocket;
        }

        public async Task HandleWebSocketAsync()
        {
            try
            {
                RaiseConnectionOpen();

                while (mWebsocket.IsConnected)
                {
                    WebSocketMessageReadStream message =
                        await mWebsocket.ReadMessageAsync(CancellationToken.None)
                            .ConfigureAwait(false);

                    if (message != null)
                    {
                        using (message)
                        {
                            WampMessage<TMessage> parsed = ParseMessage(message);
                            RaiseMessageArrived(parsed);
                        }
                    }
                }

                RaiseConnectionClosed();
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);
            }
        }

        protected abstract WampMessage<TMessage> ParseMessage(WebSocketMessageReadStream readStream);

        protected override bool IsConnected
        {
            get
            {
                return mWebsocket.IsConnected;
            }
        }

        public override void Dispose()
        {
            mWebsocket.Dispose();
        }
    }
}