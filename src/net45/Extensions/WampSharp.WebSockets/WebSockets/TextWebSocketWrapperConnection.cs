using System;
using System.Net.WebSockets;
using System.Text;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class TextWebSocketWrapperConnection<TMessage> : WebSocketWrapperConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public TextWebSocketWrapperConnection(IWebSocketWrapper webSocket, IWampTextBinding<TMessage> binding, ICookieProvider cookieProvider, ICookieAuthenticatorFactory cookieAuthenticatorFactory) : 
            base(webSocket, binding, cookieProvider, cookieAuthenticatorFactory)
        {
            mBinding = binding;
        }

        protected TextWebSocketWrapperConnection(IClientWebSocketWrapper clientWebSocket, Uri addressUri, IWampTextBinding<TMessage> binding) :
            base(clientWebSocket, addressUri, binding.Name, binding)
        {
            mBinding = binding;
        }

        protected override ArraySegment<byte> GetMessageInBytes(WampMessage<object> message)
        {
            string formatted = mBinding.Format(message);

            byte[] bytes = Encoding.UTF8.GetBytes(formatted);

            return new ArraySegment<byte>(bytes);
        }

        protected override WebSocketMessageType WebSocketMessageType
        {
            get
            {
                return WebSocketMessageType.Text;
            }
        }
    }
}