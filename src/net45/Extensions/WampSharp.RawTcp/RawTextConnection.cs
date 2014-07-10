using System;
using System.Text;
using System.Threading;
using WampSharp.Binding;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;

namespace WampSharp.RawTcp
{
    internal class RawTextConnection<TMessage> : IWampConnection<TMessage>
    {
        private readonly RawSession mConnection;
        private readonly IWampTransportBinding<TMessage, string> mBinding;
        private int mIsOpen;

        public RawTextConnection(RawSession connection, IWampTransportBinding<TMessage, string> binding)
        {
            mConnection = connection;
            mConnection.MessageArrived += OnMessageArrived;
            mBinding = binding;
        }

        private void OnMessageArrived(object sender, MessageArrivedEventArgs e)
        {
            // Maybe this should in a binding
            string text = Encoding.UTF8.GetString(e.Message);
            WampMessage<TMessage> parsed = this.mBinding.Parse(text);

            RaiseMessageArrived(new WampMessageArrivedEventArgs<TMessage>(parsed));
        }

        private void RaiseMessageArrived(WampMessageArrivedEventArgs<TMessage> wampMessageArrivedEventArgs)
        {
            if (Interlocked.CompareExchange(ref mIsOpen, 1, 0) == 0)
            {
                RaiseConnectionOpen();
            }

            EventHandler<WampMessageArrivedEventArgs<TMessage>> onMessageArrived = this.MessageArrived;
            
            if (onMessageArrived != null)
            {
                onMessageArrived(this, wampMessageArrivedEventArgs);
            }
        }

        public void Dispose()
        {
            mConnection.Close();
        }

        public void Send(WampMessage<TMessage> message)
        {
            string text = mBinding.Format(message);

            int length = text.Length;

            byte[] lengthBytes = BitConverter.GetBytes(length);
            
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBytes);
            }

            byte[] result = 
                Encoding.UTF8.GetBytes(text);

            ArraySegment<byte>[] segments =
                new[]
                    {
                        new ArraySegment<byte>(lengthBytes),
                        new ArraySegment<byte>(result)
                    };

            mConnection.Send(segments);
        }

        public event EventHandler ConnectionOpen;

        protected virtual void RaiseConnectionOpen()
        {
            EventHandler handler = ConnectionOpen;
            
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}