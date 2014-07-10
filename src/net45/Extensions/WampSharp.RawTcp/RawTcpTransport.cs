using System;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.RawTcp
{
    public class RawTcpTransport : IWampTransport<string>
    {
        private readonly RawServer mServer;
        private ConnectionListener mTextConnectionListener;

        public RawTcpTransport(int port)
        {
            mServer = new RawServer();
            mServer.NewSessionConnected += NewSessionConnected;
            mServer.Setup("127.0.0.1", port);
        }

        private void NewSessionConnected(RawSession session)
        {
            mTextConnectionListener.OnNewConnection(session);
        }

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTransportBinding<TMessage, string> binding)
        {
            IWampTextBinding<TMessage> casted =
                binding as IWampTextBinding<TMessage>;

            if (casted != null)
            {
                return GetListener(casted);
            }

            throw new ArgumentException("Expected IWampTextBinding<TMessage>",
                                        "binding");
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

        public IWampConnectionListener<TMessage> GetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            IWampTextBinding<TMessage> casted = 
                binding as IWampTextBinding<TMessage>;

            if (casted != null)
            {
                return GetListener(casted);
            }

            throw new ArgumentException("Expected IWampTextBinding<TMessage>",
                                        "binding");
        }

        private IWampConnectionListener<TMessage> GetListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            if (mTextConnectionListener == null)
            {
                TextConnectionListener<TMessage> textConnectionListener = new TextConnectionListener<TMessage>(binding);

                mTextConnectionListener = textConnectionListener;
            }

            return mTextConnectionListener as IWampConnectionListener<TMessage>;
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
}