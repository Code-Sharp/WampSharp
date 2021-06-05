using System;
using System.Net.WebSockets;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class TextWebSocketWrapperConnection<TMessage> : WebSocketWrapperConnection<TMessage, string>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public TextWebSocketWrapperConnection(IWebSocketWrapper webSocket, IWampTextBinding<TMessage> binding,
                                              ICookieProvider cookieProvider,
                                              ICookieAuthenticatorFactory cookieAuthenticatorFactory, int? maxFrameSize) : 
            base(webSocket, binding, cookieProvider, cookieAuthenticatorFactory, maxFrameSize)
        {
            mBinding = binding;
        }

        protected TextWebSocketWrapperConnection(IClientWebSocketWrapper clientWebSocket, Uri addressUri,
                                                 IWampTextBinding<TMessage> binding, int? maxFrameSize) :
            base(clientWebSocket, addressUri, binding.Name, binding, maxFrameSize)
        {
            mBinding = binding;
        }

        protected override WebSocketMessageType WebSocketMessageType => WebSocketMessageType.Text;
    }
}