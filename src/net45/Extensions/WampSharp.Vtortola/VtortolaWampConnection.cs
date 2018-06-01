using System;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.MetaApi;

namespace WampSharp.Vtortola
{
    internal abstract class VtortolaWampConnection<TMessage> : AsyncWebSocketWampConnection<TMessage>,
        IDetailedWampConnection<TMessage>
    {
        protected readonly WebSocket mWebsocket;
        private readonly CancellationToken mCancellationToken;
        private readonly VtortolaTransportDetails mTransportDetails;

        protected VtortolaWampConnection(WebSocket websocket,
            CancellationToken cancellationToken,
            ICookieAuthenticatorFactory cookieAuthenticatorFactory)
            : base(new CookieCollectionCookieProvider(websocket.HttpRequest.Cookies),
                cookieAuthenticatorFactory)
        {
            mWebsocket = websocket;
            mCancellationToken = cancellationToken;
            mTransportDetails = new VtortolaTransportDetails(mWebsocket);
        }

        public async Task HandleWebSocketAsync()
        {
            try
            {
                RaiseConnectionOpen();

                while (IsConnected)
                {
                    WebSocketMessageReadStream message =
                        await mWebsocket.ReadMessageAsync(CancellationToken.None)
                            .ConfigureAwait(false);

                    if (message != null)
                    {
                        using (message)
                        {
                            WampMessage<TMessage> parsed = await ParseMessage(message).ConfigureAwait(false);
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

        protected override bool IsConnected => !mCancellationToken.IsCancellationRequested &&
                                               mWebsocket.IsConnected;

        protected override void Dispose()
        {
            mWebsocket.Dispose();
        }

        public WampTransportDetails TransportDetails => mTransportDetails;
    }
}