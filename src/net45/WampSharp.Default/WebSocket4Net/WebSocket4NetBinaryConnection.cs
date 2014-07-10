using WampSharp.Binding;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;
using WebSocket4Net;

namespace WampSharp.WebSocket4Net
{
    public class WebSocket4NetBinaryConnection<TMessage> : WebSocket4NetConnection<TMessage>
    {
        private readonly IWampTransportBinding<TMessage, byte[]> mBinding;

        public WebSocket4NetBinaryConnection(string serverAddress, IWampTransportBinding<TMessage, byte[]> binding)
            : base(serverAddress, binding)
        {
            mBinding = binding;
            WebSocket.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            WampMessage<TMessage> message = mBinding.Parse(e.Data);

            RaiseMessageArrived(message);
        }

        public override void Send(WampMessage<TMessage> message)
        {
            byte[] bytes = mBinding.Format(message);

            WebSocket.Send(bytes, 0, bytes.Length);
        }

        public override void Dispose()
        {
            WebSocket.DataReceived -= OnDataReceived;
            base.Dispose();
        }
    }
}