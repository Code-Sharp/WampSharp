using System;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using WampSharp.Binding;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;

namespace WampSharp.RawTcp
{
    public class RawTcpListenerProvider : IWampConnectionListenerProvider
    {
        private readonly RawServer mServer;
        private ConnectionListener mTextConnectionListener;

        public RawTcpListenerProvider(int port)
        {
            mServer = new RawServer();
            mServer.NewSessionConnected += NewSessionConnected;
            mServer.Setup("127.0.0.1", port);
        }

        private void NewSessionConnected(RawSession session)
        {
            mTextConnectionListener.OnNewConnection(session);
        }
        
        public void Dispose()
        {
            mServer.Stop();
            mServer.Dispose();
        }

        public void Open()
        {
            mServer.Start();
        }

        public IWampConnectionListener<TMessage> GetTextListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            if (mTextConnectionListener == null)
            {
                TextConnectionListener<TMessage> textConnectionListener = new TextConnectionListener<TMessage>(binding);

                mTextConnectionListener = textConnectionListener;
            }

            return mTextConnectionListener as IWampConnectionListener<TMessage>;
        }

        public IWampConnectionListener<TMessage> GetBinaryListener<TMessage>(IWampBinaryBinding<TMessage> binding)
        {
            return null;
        }

        private abstract class ConnectionListener : IDisposable
        {
            public abstract void OnNewConnection(RawSession connection);
            public abstract void Dispose();
        }

        private abstract class ConnectionListener<TMessage> : ConnectionListener,
            IWampConnectionListener<TMessage>
        {
            private readonly Subject<IWampConnection<TMessage>> mSubject =
                new Subject<IWampConnection<TMessage>>();

            protected void OnNewConnection(IWampConnection<TMessage> connection)
            {
                mSubject.OnNext(connection);
            }

            public IDisposable Subscribe(IObserver<IWampConnection<TMessage>> observer)
            {
                return mSubject.Subscribe(observer);
            }

            public override void Dispose()
            {
                mSubject.OnCompleted();
                mSubject.Dispose();
            }
        }


        private class TextConnectionListener<TMessage> : ConnectionListener<TMessage>
        {
            private readonly IWampTextBinding<TMessage> mBinding;

            public TextConnectionListener(IWampTextBinding<TMessage> binding)
            {
                mBinding = binding;
            }

            public IWampTextBinding<TMessage> Binding
            {
                get
                {
                    return mBinding;
                }
            }

            public override void OnNewConnection(RawSession connection)
            {
                OnNewConnection(new RawTextConnection<TMessage>(connection, Binding));
            }
        }
    }

    internal class RawTextConnection<TMessage> : IWampConnection<TMessage>
    {
        private readonly RawSession mConnection;
        private readonly IWampTextBinding<TMessage> mBinding;
        private int mIsOpen;

        public RawTextConnection(RawSession connection, IWampTextBinding<TMessage> binding)
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