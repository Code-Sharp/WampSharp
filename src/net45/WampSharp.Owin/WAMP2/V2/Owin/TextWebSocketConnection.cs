using System;
using System.Collections.Generic;
using System.Text;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Owin
{
    public class TextWebSocketConnection<TMessage> : WebSocketConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public TextWebSocketConnection(IDictionary<string, object> webSocketContext, IWampTextBinding<TMessage> binding,
                                       ICookieProvider cookieProvider,
                                       ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
            base(webSocketContext, binding, cookieProvider, cookieAuthenticatorFactory)
        {
            mBinding = binding;
        }

        protected override ArraySegment<byte> GetMessageInBytes(WampMessage<object> message)
        {
            string formatted = mBinding.Format(message);

            byte[] bytes = Encoding.UTF8.GetBytes(formatted);

            return new ArraySegment<byte>(bytes);
        }

        protected override int MessageType
        {
            get
            {
                return WebSocketMessageType.Text;
            }
        }
    }
}