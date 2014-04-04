using WampSharp.Binding;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Listener;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    public class WebSocket4NetTextConnection<TMessage> : WebSocket4NetConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public WebSocket4NetTextConnection(string serverAddress, IWampTextBinding<TMessage> binding) : base(serverAddress, binding)
        {
            mBinding = binding;
            WebSocket.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            WampMessage<TMessage> message = mBinding.Parse(e.Message);

            RaiseMessageArrived(message);
        }

        public override void Send(WampMessage<TMessage> message)
        {
            TextMessage<TMessage> textMessage =
                mBinding.GetRawMessage(message) as TextMessage<TMessage>;

            WebSocket.Send(textMessage.Text);
        }

        public override void Dispose()
        {
            WebSocket.MessageReceived -= OnMessageReceived;
            base.Dispose();
        }
    }
}