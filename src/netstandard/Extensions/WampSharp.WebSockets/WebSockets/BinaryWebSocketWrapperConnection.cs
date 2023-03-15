using System;
using System.Net.WebSockets;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.WebSockets
{
    public class BinaryWebSocketWrapperConnection<TMessage> : WebSocketWrapperConnection<TMessage, byte[]>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public BinaryWebSocketWrapperConnection(IWebSocketWrapper webSocket, IWampBinaryBinding<TMessage> binding,
                                                ICookieProvider cookieProvider,
                                                ICookieAuthenticatorFactory cookieAuthenticatorFactory,
                                                int? maxFrameSize) : 
            base(webSocket, binding, cookieProvider, cookieAuthenticatorFactory, maxFrameSize)
        {
            mBinding = binding;
        }

        protected BinaryWebSocketWrapperConnection(IClientWebSocketWrapper clientWebSocket, Uri addressUri,
                                                   IWampBinaryBinding<TMessage> binding, int? maxFrameSize) :
            base(clientWebSocket, addressUri, binding.Name, binding, maxFrameSize)
        {
            mBinding = binding;
        }

        protected override WebSocketMessageType WebSocketMessageType => WebSocketMessageType.Binary;
    }
}