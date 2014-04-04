using Fleck;
using WampSharp.Binding;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Listener;

namespace WampSharp.Fleck
{
    internal class FleckWampBinaryConnection<TMessage> : FleckWampConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public FleckWampBinaryConnection(IWebSocketConnection webSocketConnection,
                                         IWampBinaryBinding<TMessage> binding) : 
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
            BinaryMessage<TMessage> casted = 
                mBinding.GetRawMessage(message) as BinaryMessage<TMessage>; 

            mWebSocketConnection.Send(casted.Bytes);
        }
    }
}