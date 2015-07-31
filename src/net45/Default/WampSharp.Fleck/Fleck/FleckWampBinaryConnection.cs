using System.Threading.Tasks;
using Fleck;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Fleck
{
    internal class FleckWampBinaryConnection<TMessage> : FleckWampConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public FleckWampBinaryConnection(IWebSocketConnection webSocketConnection,
                                         IWampBinaryBinding<TMessage> binding,
                                         ICookieAuthenticatorFactory cookieAuthenticatorFactory) :
                                             base(webSocketConnection,
                                                  cookieAuthenticatorFactory)
        {
            mBinding = binding;
            webSocketConnection.OnBinary = OnConnectionMessage;
        }

        private void OnConnectionMessage(byte[] bytes)
        {
            WampMessage<TMessage> parsed = mBinding.Parse(bytes);
            RaiseMessageArrived(parsed);
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            byte[] bytes = mBinding.Format(message);

            return mWebSocketConnection.Send(bytes);
        }
    }
}