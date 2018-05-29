using System;
using System.Net.WebSockets;
using System.Text;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class TextWebSocketWrapperConnection<TMessage> : WebSocketWrapperConnection<TMessage, string>
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

        protected override WebSocketMessageType WebSocketMessageType => WebSocketMessageType.Text;
    }
}