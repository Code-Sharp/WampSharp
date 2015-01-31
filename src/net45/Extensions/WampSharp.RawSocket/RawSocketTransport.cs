using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.RawSocket
{
    public class RawSocketTransport : IWampTransport
    {
        private readonly RawSocketServer mServer;
        private ConnectionListener mConnectionListener;

        public RawSocketTransport(string ip, int port)
        {
            mServer = new RawSocketServer();
            mServer.Setup(ip, port);
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
            return InnerGetListener((dynamic) binding);
        }

        private IWampConnectionListener<TMessage> InnerGetListener<TMessage>(IWampBinaryBinding<TMessage> binding)
        {
            if (mConnectionListener == null)
            {
                BinaryConnectionListener<TMessage> binaryConnectionListener = new BinaryConnectionListener<TMessage>(binding);

                SetConnection(binaryConnectionListener);
            }

            return mConnectionListener as IWampConnectionListener<TMessage>;
        }

        private IWampConnectionListener<TMessage> InnerGetListener<TMessage>(IWampTextBinding<TMessage> binding)
        {
            if (mConnectionListener == null)
            {
                TextConnectionListener<TMessage> textConnectionListener = new TextConnectionListener<TMessage>(binding);

                SetConnection(textConnectionListener);
            }

            return mConnectionListener as IWampConnectionListener<TMessage>;
        }

        private IWampConnectionListener<TMessage> InnerGetListener<TMessage>(IWampBinding<TMessage> binding)
        {
            throw new ArgumentException("Expected IWampTextBinding<TMessage> or IWampBinaryBinding<TMessage>",
                                        "binding");
        }

        private void SetConnection(ConnectionListener textConnectionListener)
        {
            mConnectionListener = textConnectionListener;

            mServer.SetConnectionListener(mConnectionListener);
        }
    }
}