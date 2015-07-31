using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Reflection;

namespace WampSharp.Vtortola
{
    internal abstract class VtortolaWampConnection<TMessage> : AsyncWampConnection<TMessage>, IDetailedWampConnection<TMessage>
    {
        protected readonly WebSocket mWebsocket;
        private readonly VtortolaTransportDetails mTransportDetails;

        protected VtortolaWampConnection(WebSocket websocket)
        {
            mWebsocket = websocket;
            mTransportDetails = new VtortolaTransportDetails(mWebsocket);
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
                            WampMessage<TMessage> parsed = await ParseMessage(message);
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

        protected abstract Task<WampMessage<TMessage>> ParseMessage(WebSocketMessageReadStream readStream);

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

        public WampTransportDetails TransportDetails
        {
            get
            {
                return mTransportDetails;
            }
        }
    }
}