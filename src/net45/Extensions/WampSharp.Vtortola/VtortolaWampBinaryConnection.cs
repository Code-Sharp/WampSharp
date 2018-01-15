using System.IO;
using System.Threading.Tasks;
using vtortola.WebSockets;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Vtortola
{
    internal class VtortolaWampBinaryConnection<TMessage> : VtortolaWampConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public VtortolaWampBinaryConnection(WebSocket connection,
                                            IWampBinaryBinding<TMessage> binding,
                                            ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
                                                base(connection, cookieAuthenticatorFactory)
        {
            mBinding = binding;
        }

        protected async override Task<WampMessage<TMessage>> ParseMessage(WebSocketMessageReadStream readStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await readStream.CopyToAsync(memoryStream).ConfigureAwait(false);
                byte[] bytes = memoryStream.ToArray();
                WampMessage<TMessage> result = mBinding.Parse(bytes);
                return result;
            }
        }

        protected async override Task SendAsync(WampMessage<object> message)
        {
            using (WebSocketMessageWriteStream stream = 
                mWebsocket.CreateMessageWriter(WebSocketMessageType.Binary))
            {
                byte[] raw = mBinding.Format(message);
                await stream.WriteAsync(raw, 0, raw.Length).ConfigureAwait(false);
            }
        }
    }
}