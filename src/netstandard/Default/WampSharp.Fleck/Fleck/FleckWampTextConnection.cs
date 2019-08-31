using System.Threading.Tasks;
using Fleck;
using WampSharp.Core.Message;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;

namespace WampSharp.Fleck
{
    internal class FleckWampTextConnection<TMessage> : FleckWampConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public FleckWampTextConnection(IWebSocketConnection webSocketConnection,
                                       IWampTextBinding<TMessage> binding,
                                       ICookieAuthenticatorFactory cookieAuthenticatorFactory = null)
            : base(webSocketConnection, cookieAuthenticatorFactory)
        {
            mBinding = binding;
            webSocketConnection.OnMessage = OnConnectionMessage;
        }

        private void OnConnectionMessage(string message)
        {
            WampMessage<TMessage> parsed =
                mBinding.Parse(message);

            RaiseMessageArrived(parsed);
        }

        protected override Task SendAsync(WampMessage<object> message)
        {
            string raw = mBinding.Format(message);

            return mWebSocketConnection.Send(raw);
        }
    }
}