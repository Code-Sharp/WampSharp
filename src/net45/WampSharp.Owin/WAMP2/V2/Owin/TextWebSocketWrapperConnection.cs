using System;
using System.Net.WebSockets;
using System.Text;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Owin
{
    public class TextWebSocketWrapperConnection<TMessage> : WebSocketWrapperConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public TextWebSocketWrapperConnection(IWebSocketWrapper wrapper,
                                              IWampTextBinding<TMessage> binding,
                                              ICookieProvider cookieProvider,
                                              ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
            base(wrapper, binding, cookieProvider, cookieAuthenticatorFactory)
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