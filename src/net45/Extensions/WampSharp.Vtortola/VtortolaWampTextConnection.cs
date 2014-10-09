using System.IO;
using vtortola.WebSockets;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;

namespace WampSharp.Vtortola
{
    internal class VtortolaWampTextConnection<TMessage> : VtortolaWampConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public VtortolaWampTextConnection(WebSocket connection, IWampTextBinding<TMessage> binding) : 
            base(connection)
        {
            mBinding = binding;
        }

        protected override WampMessage<TMessage> ParseMessage(WebSocketMessageReadStream readStream)
        {
            using (StreamReader streamReader = new StreamReader(readStream))
            {
                string raw = streamReader.ReadToEnd();
                WampMessage<TMessage> result = mBinding.Parse(raw);
                return result;
            }
        }

        public override void Send(WampMessage<TMessage> message)
        {
            string raw = mBinding.Format(message);

            using (WebSocketMessageWriteStream stream =
                mWebsocket.CreateMessageWriter(WebSocketMessageType.Text))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(raw);
                }
            }
        }
    }
}