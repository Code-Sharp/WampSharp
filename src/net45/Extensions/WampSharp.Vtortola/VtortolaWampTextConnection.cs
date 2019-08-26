using System.IO;
using System.Threading;
using System.Threading.Tasks;
using vtortola.WebSockets;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Vtortola
{
    internal class VtortolaWampTextConnection<TMessage> : VtortolaWampConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public VtortolaWampTextConnection(WebSocket connection,
            CancellationToken cancellationToken,
            IWampTextBinding<TMessage> binding,
            ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
            base(connection, cancellationToken, cookieAuthenticatorFactory)
        {
            mBinding = binding;
        }

        protected override async Task<WampMessage<TMessage>> ParseMessage(WebSocketMessageReadStream readStream)
        {
            using (StreamReader streamReader = new StreamReader(readStream))
            {
                string raw = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                WampMessage<TMessage> result = mBinding.Parse(raw);
                return result;
            }
        }

        protected override async Task SendAsync(WampMessage<object> message)
        {
            string raw = mBinding.Format(message);

            using (WebSocketMessageWriteStream stream =
                mWebsocket.CreateMessageWriter(WebSocketMessageType.Text))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    await streamWriter.WriteAsync(raw)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}