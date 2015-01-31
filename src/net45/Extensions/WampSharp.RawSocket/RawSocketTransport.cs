using System;
using WampSharp.Core.Listener;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;

namespace WampSharp.RawSocket
{
    public class RawSocketTransport : IWampTransport<string>
    {
        private readonly RawServer mServer;
        private ConnectionListener mTextConnectionListener;

        public RawSocketTransport(string ip, int port)
        {
            mServer = new RawServer();
            mServer.Setup(ip, port);
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

                mServer.SetConnectionListener(mTextConnectionListener);
            }

            return mTextConnectionListener as IWampConnectionListener<TMessage>;
        }
    }

}