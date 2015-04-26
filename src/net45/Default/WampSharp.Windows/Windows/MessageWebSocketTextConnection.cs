#if PCL
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace WampSharp.Windows
{
    public class MessageWebSocketTextConnection<TMessage> : MessageWebSocketConnection<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mTextBinding;

        public MessageWebSocketTextConnection(string uri, IWampTextBinding<TMessage> binding) : 
            base(uri, binding, SocketMessageType.Utf8)
        {
            mTextBinding = binding;
        }

        protected override void OnMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                Stream stream = args.GetDataStream().AsStreamForRead();

                StreamReader reader = new StreamReader(stream);

                string frame = reader.ReadToEnd();

                WampMessage<TMessage> message = mTextBinding.Parse(frame);

                RaiseMessageArrived(message);
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);

                if (mWebSocket != null)
                {
                    mWebSocket.Dispose();                    
                }
            }
        }

        protected override async Task SendAsync(WampMessage<object> message)
        {
            try
            {
                Stream stream = mWebSocket.OutputStream.AsStreamForWrite();

                string frame = mTextBinding.Format(message);

                StreamWriter streamWriter = new StreamWriter(stream);

                await streamWriter.WriteAsync(frame);

                await streamWriter.FlushAsync();
            }
            catch (Exception ex)
            {
                RaiseConnectionError(ex);

                if (mWebSocket != null)
                {
                    mWebSocket.Dispose();
                }
                
                throw;
            }
        }
    }
}
#endif