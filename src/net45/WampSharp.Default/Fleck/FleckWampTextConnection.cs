using Fleck;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.Fleck
{
    internal class FleckWampTextConnection<TMessage> : FleckWampConnection<TMessage>
    {
        private readonly IWampTransportBinding<TMessage, string> mBinding;

        public FleckWampTextConnection(IWebSocketConnection webSocketConnection,
                                       IWampTransportBinding<TMessage, string> binding)
            : base(webSocketConnection)
        {
            mBinding = binding;
            webSocketConnection.OnMessage = OnConnectionMessage;
        }

        private void OnConnectionMessage(string message)
        {
            WampMessage<TMessage> parsed =
                mBinding.Parse(message);

            RaiseNewMessageArrived(parsed);
        }

        protected override void InnerSend(WampMessage<TMessage> message)
        {
            string raw = mBinding.Format(message);

            mWebSocketConnection.Send(raw);
        }
    }
}