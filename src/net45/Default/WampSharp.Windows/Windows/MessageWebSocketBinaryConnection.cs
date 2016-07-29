#if PCL
using System;
using System.IO;
using System.Threading.Tasks;
using WampSharp.V2.Binding;
using Windows.Networking.Sockets;
using WampSharp.Core.Message;

namespace WampSharp.Windows
{
    public class MessageWebSocketBinaryConnection<TMessage> : MessageWebSocketConnection<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinaryBinding;

        public MessageWebSocketBinaryConnection(string uri, IWampBinaryBinding<TMessage> binding) :
            base(uri, binding, SocketMessageType.Binary)
        {
            mBinaryBinding = binding;
        }

        // Regarding the async modifier - see https://github.com/Code-Sharp/WampSharp/issues/122
        protected override async void OnMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                Stream stream = args.GetDataStream().AsStreamForRead();

                MemoryStream memoryStream = new MemoryStream();

                await stream.CopyToAsync(memoryStream);

                stream.Position = 0;

                WampMessage<TMessage> message = mBinaryBinding.Parse(stream);

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

                byte[] frame = mBinaryBinding.Format(message);

                await stream.WriteAsync(frame, 0, frame.Length).ConfigureAwait(false);

                await stream.FlushAsync().ConfigureAwait(false);
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