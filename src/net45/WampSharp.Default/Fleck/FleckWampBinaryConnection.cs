using Fleck;
using WampSharp.Binding;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Fleck
{
    internal class FleckWampBinaryConnection<TMessage> : FleckWampConnection<TMessage>
    {
        private readonly IWampTransportBinding<TMessage, byte[]> mBinding;

        public FleckWampBinaryConnection(IWebSocketConnection webSocketConnection,
                                         IWampTransportBinding<TMessage, byte[]> binding) : 
                                             base(webSocketConnection)
        {
            mBinding = binding;
            webSocketConnection.OnBinary = OnConnectionMessage;
        }

        private void OnConnectionMessage(byte[] bytes)
        {
            WampMessage<TMessage> parsed = mBinding.Parse(bytes);
            RaiseNewMessageArrived(parsed);
        }

        protected override void InnerSend(WampMessage<TMessage> message)
        {
            byte[] bytes =
                mBinding.Format(message); 

            mWebSocketConnection.Send(bytes);
        }
    }
}